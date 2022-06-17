using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demon : Entity
{
    [SerializeField] private int hp;		 	
	[SerializeField] private Transform point;	
	[SerializeField] private LayerMask PlayerLayer;
	[SerializeField] private LayerMask GroundLayer;
	[SerializeField] private CapsuleCollider2D capsulIdle;
	[SerializeField] private CapsuleCollider2D capsulAttack;
	[SerializeField] private BoxCollider2D boxDie;							

	private float speed;
	private float damageDemon = 2;		
	private float agrDist = 15;
	private float attackRange = 3f;
	private float jumpForce;
	private float cdJump;
	private float RayDistToGround = 1.5f;		
	private int FullHP;	
	private Transform player;		

	private bool NotDie = true;	
	private bool cd;	
	private bool canAttack;
	private bool isGround;
	private bool isWall;
	private bool isWallR;
	private bool isWallL;
	private bool idle = true;
	private bool agr = false;	
	private bool attack = false;		

	private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private Animator anim;	
	public ProgressBar Pb;
	private RaycastHit2D isCheckGround;
	private RaycastHit2D isCheckWallL;
	private RaycastHit2D isCheckWallR;

	//Программные функции	

 	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponent<Animator>();		
		
		player = GameObject.FindGameObjectWithTag("Player").transform;
		FullHP = hp;
		cd = false;		
		cdJump = 0;
		boxDie.enabled = false;
	}
	
	private void FixedUpdate()
	{
		CheckPlayer();
		CheckGround();				
	}
	
	private void Update()
	{
		if (hp <= 0 && NotDie) WhenDie();
		Pb.BarValue = hp*(100/FullHP);		

		if (cdJump > 0) cdJump -= Time.deltaTime;
		if (!isGround) jumpForce = 15; else jumpForce = 7;  	

		if (NotDie && !canAttack && agr == false) idle = true; 		
		if (NotDie && !canAttack && Vector2.Distance(transform.position, player.position) < agrDist) {agr = true; idle = false;} 		
		if (NotDie && !cd && canAttack) {attack = true; idle = false; agr = false;}
		if (NotDie && isWall) Jump();
						

		if (idle == true) {Idle(); capsulIdle.enabled = true; capsulAttack.enabled = false;}
		 else if (attack == true) {Attack(); capsulIdle.enabled = false; capsulAttack.enabled = true; attack = false;}		  
		  else if (agr == true) {Agr(); capsulIdle.enabled = true; capsulAttack.enabled = false; agr = false;}		  		   
	}

	//Основные функции
			
	private void Idle()
	{
		speed = 0;
		anim.SetBool("isWalk", false);		
		sprite.flipX = true;		
	}

	private void Agr()
	{
		speed = 2;		
		anim.SetBool("isWalk", true);						
		transform.position = Vector2.MoveTowards(transform.position, player.position, speed*Time.deltaTime);
		if (transform.position.x > player.position.x) sprite.flipX = true; else sprite.flipX = false;		
	}	

	private void Jump()
	{
		if (cdJump <= 0)
		{
		rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
		cdJump = 2f;		
		}			
	}	

	private void Attack()
	{
		anim.SetTrigger("isAttack");						
		cd = true;			
		StartCoroutine(AttackCoolDown());				
	}

	private void OnAttack()
	{
		speed = 0;
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, PlayerLayer);
		for (int i = 0; i<enemies.Length; i++)
		{
			enemies[i].GetComponent<Hero>().GetDamage(damageDemon);
		}
	}

	private  IEnumerator AttackCoolDown()
	{
		yield return new WaitForSeconds(1f);
		cd = false;
	}	

	//Вспомогательные функции

	private void CheckPlayer()
	{
		if (Vector2.Distance(transform.position, player.position) <= attackRange) canAttack = true; else canAttack = false;
	}

	private void CheckGround()
	{
		isCheckGround = Physics2D.Raycast(transform.position, -Vector2.up, RayDistToGround, GroundLayer);					
		isGround = isCheckGround;

		isCheckWallR = Physics2D.Raycast(transform.position, Vector2.right, RayDistToGround, GroundLayer);
		isCheckWallL = Physics2D.Raycast(transform.position, -Vector2.right, RayDistToGround, GroundLayer);					
		isWallR = isCheckWallR;
		isWallL = isCheckWallL;				
		isWall = isWallR || isWallL;
	}

	public override void Damage(int damage)
	{
		hp -= damage;
	}

	private void WhenDie()
	{
		NotDie = false;
		boxDie.enabled = true;
		capsulIdle.enabled = false;
		capsulAttack.enabled = false;		
		anim.SetTrigger("isDie");		
		Invoke("Die",3);
		Invoke("Respawn",6);		
	}

	private void Respawn()
   {
        this.gameObject.SetActive(true);		
		hp = FullHP;
		transform.position = point.position;
		NotDie = true;
		cd = false;
		boxDie.enabled = false;							
   }

   private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Border") {Die(); Invoke("Respawn", 6);}
	}
   
}

