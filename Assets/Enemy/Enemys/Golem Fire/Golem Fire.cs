using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private int hp;
	[SerializeField] private float speed;	
	[SerializeField] private Transform point;
	
	private float speed1 = 10;
	private int distOfPatrol = 5;
	private float agrDist = 8;	
	private float FullHP;
	private Transform player;		

	private bool NotDie = true;
	private bool moveRigth = true;
	private bool walk = false;
	private bool agr = false;
	private bool back = false;

	private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private Animator anim;
	public ProgressBar Pb;

	//Программные функции	

 	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponent<Animator>();		
		
		player = GameObject.FindGameObjectWithTag("Player").transform;
		FullHP = hp;
	}
	
	private void FixedUpdate()
	{
		if (hp <= 0) WhenDie();
		Pb.BarValue = hp*(100/FullHP);
			
	}
	
	
	private void Update()
	{
		if (NotDie && agr == false && Vector2.Distance(transform.position, point.position) < distOfPatrol+1) walk = true;		
		if (NotDie && Vector2.Distance(transform.position, player.position) < agrDist) {agr = true; walk = false; back = false;}
		if (NotDie && Vector2.Distance(transform.position, player.position) > agrDist) {back = true; agr = false;}
				
		if (walk == true) Walk(); 
		 else if (agr == true) Agr(); 
		  else if (back == true) GoBack(); 
	}

	private void OnCollisionEnter2D(Collision2D enemy)
	{
		if (enemy.transform.tag == "Player")
		{ 
			Hero.Instance.GetDamage();	//Сделать разный урон от разных атак		
		}		
	}
	
	//Основные функции
			
	private void Walk()
	{
		anim.SetBool("isWalk", true);
		if (transform.position.x > point.position.x + distOfPatrol) moveRigth = false;
		  else if (transform.position.x < point.position.x - distOfPatrol) moveRigth = true;

		if (moveRigth)  {transform.position = new Vector2(transform.position.x + speed*Time.deltaTime, transform.position.y); sprite.flipX = false;}
		  else	{transform.position = new Vector2(transform.position.x - speed*Time.deltaTime, transform.position.y); sprite.flipX = true;}
	}

	private void Agr()
	{
		anim.SetBool("isWalk", true);		
		transform.position = Vector2.MoveTowards(transform.position, player.position, speed1*Time.deltaTime);
		if (transform.position.x > player.position.x) sprite.flipX = true; else sprite.flipX = false;
	}

	private void GoBack()
	{
		anim.SetBool("isWalk", true);
		transform.position = Vector2.MoveTowards(transform.position, point.position, speed*Time.deltaTime);
		if (transform.position.x > point.position.x) sprite.flipX = true; else sprite.flipX = false;
	}

	//Вспомогательные функции

	public void GetDamage1(int damage1)
	{
		hp -= damage1;
	}

	private void WhenDie()
	{
		anim.SetTrigger("isDie");
		NotDie = false;
		Invoke("Die",5);
	}

	
}
