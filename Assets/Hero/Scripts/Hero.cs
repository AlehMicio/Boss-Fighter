using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hero : Entity
{	
		
	[SerializeField] private float speed;
	[SerializeField] private float jumpForce;
	[SerializeField] private float hp; 
			
	private float speedX;
	private float speedY;
	private int damage1 = 5;	
	private float FullHP;
	private bool isGround = false;	
	private bool NotDie = true;
	private bool cd; //CoolDown

	[SerializeField] private Transform AttackPoint;
	[SerializeField] private float AttackRange;
	[SerializeField] private LayerMask enemy;	
	
	private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private Animator anim;
	public ProgressBar Pb;
	
	public static Hero Instance {get; set;}
	
	//Программные функции
 	
	private void Start()
	{
		Instance = this;			
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponent<Animator>();

		cd = true;
		FullHP = hp;											
	}
	
	private void FixedUpdate()
	{
		CheckGround();
		if (hp <= 0) WhenDie();
		Pb.BarValue = hp*(100/FullHP); //Корректровка HP Bar	

		if (NotDie && isGround && Input.GetButton("Horizontal")) anim.SetBool("isRun", true); else anim.SetBool("isRun", false);
		if (NotDie && !isGround) anim.SetBool("isJump", true); else anim.SetBool("isJump", false);
		if (NotDie && isGround && Input.GetKey(KeyCode.S)) anim.SetBool("isSit", true); else anim.SetBool("isSit", false);
	}
	
	private void Update()
	{
		if (NotDie && Input.GetButton("Horizontal")) Run();		  
		if (NotDie && isGround && Input.GetButtonDown("Jump")) Jump();
		if (NotDie && isGround && Input.GetKey(KeyCode.S)) Sit();
		if (NotDie && Input.GetButtonDown("Fire1")) Attack(); 	 						
	}
	
	//Основные функции
	
	private void Run()
	{
		speedX = Input.GetAxis("Horizontal");
		Vector3 dir = transform.right*speedX;
		transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed*Time.deltaTime);		
		sprite.flipX = dir.x < 0.0f;		
	}
	
	private void Jump()
	{
		rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);		
	}
	
	private void Sit()  //Будет доработана (добавить коллайдоры)
	{
		
	}

	private void Attack()
	{
		anim.SetTrigger("isAttack");
		if (isGround && cd)
		{
			Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, enemy);
			for (int i = 0; i<enemies.Length; i++)
			{
				enemies[i].GetComponent<Enemy>().GetDamage1(damage1);
			}	

			cd = false;			
		 	StartCoroutine(AttackCoolDown());
		}

	}

	private  IEnumerator AttackCoolDown()
	{
		yield return new WaitForSeconds(0.5f);
		cd = true;
	}


	//Вспомогательные функции

	private void CheckGround()
	{
		Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1f);		
		isGround = collider.Length > 1;				
	}
	
	public override void GetDamage()
	{
		hp -= 1;					
	}		
	
	private void WhenDie()
	{
		anim.SetTrigger("isDie");
		NotDie = false;
		Invoke("Die",3);
		Invoke("ReloadLevel",3);
	}	


	//Функции интрфейса

	private void ReloadLevel()  //Respawn
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);				
	}

	// private void OnGUI() //Временный HP Bar
	// {
	// 	GUI.Box(new Rect (0,0,100,30), "HP = " + hp);
	// }

	private void OnDrawGizmosSelected() //Сфера для радиуса атаки
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
	}
	
}

			
	


