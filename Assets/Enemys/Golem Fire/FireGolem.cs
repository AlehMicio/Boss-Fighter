using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireGolem : Entity
{
    [SerializeField] private int hp;
	[SerializeField] private float speed;	
	[SerializeField] private Transform point;	
	[SerializeField] private LayerMask PlayerLayer;
	[SerializeField] public Text txt;
	
	private float speed1 = 15;
	private int damageFireGolem = 1;	
	private float agrDist = 8;
	private float attackRange = 3;	
	private float FullHP;
	private Transform player;		

	private bool NotDie = true;
	//private bool moveRigth = true;
	private bool cd;
	private bool canAttack;
	private bool idle = false;
	private bool agr = false;
	private bool back = false;		

	private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private Animator anim;	
	//public ProgressBar Pb;

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
	}
	
	private void Update()
	{
		if (hp <= 0) WhenDie();
		//Pb.BarValue = hp*(100/FullHP);

		if (NotDie && agr == false && !canAttack && Vector2.Distance(transform.position, point.position) < agrDist) idle = true;		
		if (NotDie && !canAttack && Vector2.Distance(transform.position, player.position) < agrDist) {agr = true; idle = false; back = false;}
		if (NotDie && !canAttack && Vector2.Distance(transform.position, player.position) > agrDist) {back = true; agr = false;}
		if (NotDie && !cd && canAttack) {Attack(); agr = false; back = false;}
				
		if (idle == true) Idle();		  
		  else if (agr == true) Agr();		  
		   else if (back == true) GoBack(); 
	}

	//Основные функции
			
	private void Idle()
	{
		//Чилим
	}

	private void Agr()
	{
		anim.SetBool("isRun", true);		
		txt.text = "Я тебе щас ебало разобью!";
		transform.position = Vector2.MoveTowards(transform.position, player.position, speed1*Time.deltaTime);
		if (transform.position.x > player.position.x) sprite.flipX = true; else sprite.flipX = false;
	}

	private void GoBack()
	{
		anim.SetBool("isWalk", true);
		transform.position = Vector2.MoveTowards(transform.position, point.position, speed*Time.deltaTime);
		if (transform.position.x > point.position.x) sprite.flipX = true; else sprite.flipX = false;
	}

	private void Attack()
	{
		anim.SetTrigger("isAttack");
		
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, PlayerLayer);
		for (int i = 0; i<enemies.Length; i++)
		{
			enemies[i].GetComponent<Hero>().GetDamage(damageFireGolem); //Hero - a name of script;
		}	

		cd = true;			
		StartCoroutine(AttackCoolDown());
		
	}

	private  IEnumerator AttackCoolDown()
	{
		yield return new WaitForSeconds(3f);
		cd = false;
	}

	//Вспомогательные функции

	private void CheckPlayer()
	{
		if (Vector2.Distance(transform.position, player.position) <= attackRange) canAttack = true; else canAttack = false;
	}


	public void GetDamage(int damageHero)
	{
		hp -= damageHero;
	}

	private void WhenDie()
	{
		anim.SetTrigger("isDie");
		NotDie = false;
		Invoke("Die",5);
	}

	private void OnDrawGizmosSelected() //Сфера для радиуса атаки
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);	
	}

}

