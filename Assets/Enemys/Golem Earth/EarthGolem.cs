using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthGolem: Entity
{
    [SerializeField] private int hp;		 	
	[SerializeField] private Transform point;
	[SerializeField] private Transform RazgonPoint;
	[SerializeField] private PointForRazgon RP;
	[SerializeField] private LayerMask PlayerLayer;
	[SerializeField] private LayerMask GroundLayer;
	[SerializeField] public Text txt;						

	private float speed;
	private float damageEarthGolem1 = 1;
	private float damageEarthGolem2 = 5;	
	private float agrDist = 5;
	private float attackRange = 1.5f;
	private float jumpForce = 0.2f;
	private float RayDistToGround = 1.5f;	
	private int FullHP;	
	private Transform player;
			

	[HideInInspector] public bool NotDie = true;	
	private bool cd;
	private bool canAttack;
	private bool isGround;
	private bool isWall;
	private bool isWallR;
	private bool isWallL;
	private bool idle = true;
	private bool agr = false;
	private bool back = false;
	private bool attack = false;
	[HideInInspector] public bool razgon;		

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

		if (NotDie && !canAttack && agr == false && Vector2.Distance(transform.position, point.position) < 1) idle = true; 		
		if (NotDie && !canAttack && Vector2.Distance(transform.position, player.position) < agrDist) {agr = true; idle = false; back = false;} 
		if (NotDie && !canAttack && agr == false && idle == false && Vector2.Distance(transform.position, player.position) > agrDist) {back = true; agr = false; idle = false; attack = false;}	
		if (NotDie && !cd && canAttack) {attack = true; idle = false; agr = false;}
		if (NotDie && isWall) Jump();		
				
		if (razgon == true)
		{
			if (transform.position.x != RazgonPoint.position.x) Razgon();
			 else razgon = false; 
		}
		else if (idle == true) Idle();
		 else if (attack == true) {Attack(); attack = false;}		  
		  else if (agr == true) {Agr(); agr = false;}				  
		   else if (back == true)
		     if (Vector2.Distance(transform.position, point.position) < 20)	{GoBack(); back = false;}
			  else
			   {
				   NotDie = false;
				   back = false;
				   Die();				   
				   Invoke ("Respawn",3);				   
			   }	
	}

	//Основные функции
			
	private void Idle()
	{
		speed = 0;
		anim.SetBool("isWalk", false);
		anim.SetBool("isRun", false);		
		sprite.flipX = true;
		txt.text = "Пока на расслабоне, на чиле";
	}

	private void Agr()
	{
		speed = 4;		
		anim.SetBool("isRun", true);
		txt.text = "Ща захуярю";				
		transform.position = Vector2.MoveTowards(transform.position, player.position, speed*Time.deltaTime);
		if (transform.position.x > player.position.x) sprite.flipX = true; else sprite.flipX = false;		
	}

	private void GoBack()
	{
		speed = 8;
		anim.SetBool("isWalk", true);
		txt.text = "На базу";
		transform.position = Vector2.MoveTowards(transform.position, point.position, speed*Time.deltaTime);
		if (transform.position.x >= point.position.x) sprite.flipX = true; else sprite.flipX = false;
	}

	private void Jump()
	{
		rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
	}

	private void Attack()
	{
		anim.SetTrigger("isAttack");
		txt.text = "Атакую!";				
		cd = true;			
		StartCoroutine(AttackCoolDown());				
	}

	private void OnAttack()
	{
		speed = 0;
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, PlayerLayer);
		for (int i = 0; i<enemies.Length; i++)
		{
			enemies[i].GetComponent<Hero>().GetDamage(damageEarthGolem1); 
		}
	}

	private  IEnumerator AttackCoolDown()
	{
		yield return new WaitForSeconds(1f);
		cd = false;
	}

	private void Razgon()
	{				
		anim.SetTrigger("isRazgon");
		txt.text = "УУУУУУУУУУУУУУУ!";
		speed = 20;
		transform.position = Vector2.MoveTowards(transform.position, RazgonPoint.position, speed*Time.deltaTime);					
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, PlayerLayer);
		for (int i = 0; i<enemies.Length; i++)
		{
			enemies[i].GetComponent<Hero>().Otdacha(damageEarthGolem2); 
		}					
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
		txt.text = "";
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
		razgon = false;
		cd = false;
		RP.GetComponent<PointForRazgon>().RespawnRazgonPoint();
   }

}