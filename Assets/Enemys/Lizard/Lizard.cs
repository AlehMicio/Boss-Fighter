using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lizard: Entity
{
    [SerializeField] private int hp;		 	
	[SerializeField] private Transform point;	
	[SerializeField] private LayerMask PlayerLayer;
	[SerializeField] private LayerMask GroundLayer;
	[SerializeField] private CircleCollider2D circle;
	[SerializeField] private CapsuleCollider2D capsulDie;								

	private float speed;
	private float damageLizard = 1;		
	private float agrDist = 10;
	private int distOfPatrol = 5;
	private float attackRange = 1.5f;
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
	private bool moveRigth = true;
	private bool patrol = true;
	private bool agr = false;
	private bool attack = false;
	private bool back = false;		

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

		if (NotDie && !canAttack && agr == false && Vector2.Distance(transform.position, point.position) < distOfPatrol+1) patrol = true; 		
		if (NotDie && !canAttack && Vector2.Distance(transform.position, player.position) < agrDist) {agr = true; patrol = false;}
		if (NotDie && !canAttack && agr == false && patrol == false && Vector2.Distance(transform.position, player.position) > agrDist) {back = true; agr = false; attack = false;}			
		if (NotDie && !cd && canAttack) {attack = true; patrol = false; agr = false;}
		if (NotDie && isWall) Jump();
						

		if (patrol == true) Patrol();
		 else if (attack == true) {Attack(); attack = false; }		  
		  else if (agr == true) {Agr(); agr = false;}
		   else if (back == true)
		     if (Vector2.Distance(transform.position, point.position) < 15)	{GoBack(); back = false;}
			  else
			   {
				   this.gameObject.SetActive(false);
				   back = false;
				   Invoke("Respawn",3);				   
			   }		  
		   
	}

	//Основные функции
			
	private void Patrol()
	{
		speed = 3;
		anim.SetBool("isWalk", true);
		if (transform.position.x > point.position.x + distOfPatrol) moveRigth = false;
		  else if (transform.position.x < point.position.x - distOfPatrol) moveRigth = true;

		if (moveRigth)  {transform.position = new Vector2(transform.position.x + speed*Time.deltaTime, transform.position.y); sprite.flipX = false;}
		  else	{transform.position = new Vector2(transform.position.x - speed*Time.deltaTime, transform.position.y); sprite.flipX = true;}						
	}

	private void Agr()
	{
		speed = 4;
		anim.SetBool("isWalk", true);									
		transform.position = Vector2.MoveTowards(transform.position, player.position, speed*Time.deltaTime);
		if (transform.position.x > player.position.x) sprite.flipX = true; else sprite.flipX = false;		
	}

	private void GoBack()
	{
		speed = 4;				
		transform.position = Vector2.MoveTowards(transform.position, point.position, speed*Time.deltaTime);
		if (transform.position.x >= point.position.x) sprite.flipX = true; else sprite.flipX = false;
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
		//anim.SetBool("isWalk", false);
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
			enemies[i].GetComponent<Hero>().GetDamage(damageLizard); 
		}
	}

	private  IEnumerator AttackCoolDown()
	{
		yield return new WaitForSeconds(0.5f);
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
		capsulDie.enabled = true;
		circle.enabled = false;
		anim.SetTrigger("isDie");		
		Invoke("Die",3);
		//Invoke("Respawn",6);		
	}

	private void Respawn()
   {
        this.gameObject.SetActive(true);		
		hp = FullHP;
		transform.position = point.position;
		NotDie = true;
		cd = false;							
   }

   private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Border") {Die(); Invoke("Respawn", 6);}
	}
   
}
