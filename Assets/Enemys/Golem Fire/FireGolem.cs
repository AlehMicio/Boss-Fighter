using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireGolem: Entity
{
    [SerializeField] private int hp;
	//[SerializeField] private GameObject Prefab;	 	
	[SerializeField] private Transform point;	
	[SerializeField] private LayerMask PlayerLayer;
	[SerializeField] private LayerMask GroundLayer;
	[SerializeField] public Text txt;		
	
	private float speed;
	private int damageFireGolem = 1;	
	private float agrDist = 5;
	private float attackRange = 1.5f;
	private float jumpForce = 0.2f;
	private float RayDistToGround = 1.5f;	
	private float FullHP;	
	private Transform player;		

	private bool NotDie = true;
	//private bool moveRigth = true;
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

		if (hp <= 0 && NotDie) WhenDie();
		Pb.BarValue = hp*(100/FullHP); 			
	}
	
	private void Update()
	{
		if (NotDie && !canAttack && agr == false && Vector2.Distance(transform.position, point.position) < 1) idle = true; 		
		if (NotDie && !canAttack && Vector2.Distance(transform.position, player.position) < agrDist) {agr = true; idle = false; back = false;} 
		if (NotDie && !canAttack && idle == false && Vector2.Distance(transform.position, player.position) > agrDist) {back = true; agr = false; idle = false; attack = false;}
		if (NotDie && !cd && canAttack) {attack = true; idle = false; agr = false;}
		if (NotDie && isWall) Jump();				

		if (idle == true) Idle();
		 else if (attack == true) {Attack(); attack = false; }		  
		  else if (agr == true) {Agr(); agr = false;}		  
		   else if (back == true) {GoBack(); back = false;}	
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
		speed = 4;
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
			enemies[i].GetComponent<Hero>().GetDamage(damageFireGolem); //Hero - a name of script;
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
		txt.text = "";
		anim.SetTrigger("isDie");		
		Invoke("Die",5);
		//Invoke("Respawn",3);		
		
	}

	private void Respawn ()
   {
       //Instantiate(Prefab, point.position, Quaternion.identity);
   }

	private void OnDrawGizmosSelected() //Сфера для радиуса атаки
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);	
	}

}
