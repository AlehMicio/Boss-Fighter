using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jinn: Entity
{
    [SerializeField] private int hp;		 	
	[SerializeField] private Transform point;
	[SerializeField] private Transform attackPoint;	
	[SerializeField] private LayerMask PlayerLayer;							
	[SerializeField] private GameObject jinnBlast;	
	[SerializeField] private Text txt;
	[SerializeField] private CapsuleCollider2D capsul;

	private float cdFire;			
	private int FullHP;	
	private Transform player;		

	private bool NotDie = true;
	private bool fire;							

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
		cdFire = 0;
	}
	
	private void FixedUpdate()
	{
		CheckPlayer();						
	}
	
	private void Update()
	{
		if (hp <= 0 && NotDie) WhenDie();
		Pb.BarValue = hp*(100/FullHP);	

		if (NotDie && fire) Fire();
	}

	//Основные функции		

	private void Fire()
	{
		if (cdFire <= 0)
		{						
			txt.text = "Я Рембо нах!";
			anim.SetTrigger("isAttack");			
			Instantiate(jinnBlast, attackPoint.position, attackPoint.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}	

	//Вспомогательные функции

	private void CheckPlayer()
	{
		if (Vector2.Distance(transform.position, player.position) < 13f)
		 fire = true; 
		  else fire = false; 
	}	

	public override void Damage(int damage)
	{
		hp -= damage;
	}

	private void WhenDie()
	{
		NotDie = false;
		txt.text = "Я был Рембо нах...";		
		anim.SetTrigger("isDeath");		
		Invoke("Die",2);
		//Invoke("Respawn",4);
		capsul.enabled = false;		
	}

	private void Respawn()
   {
        this.gameObject.SetActive(true);		
		hp = FullHP;
		transform.position = point.position;
		NotDie = true;
		capsul.enabled = true;									
   }

   private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Border") {Die(); Invoke("Respawn", 4);}
	}
   
}

