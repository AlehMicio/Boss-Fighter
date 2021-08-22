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
			
	private float naprX;	
	private int damageHero = 5;	
	private float FullHP;
	private float RayDistToGround = 1f;
	
	private bool isGround;
	private bool isSit;	
	private bool NotDie = true;			
	private bool cd; //CoolDown

	[SerializeField] private Transform AttackPoint;
	[SerializeField] private float AttackRange;
	[SerializeField] private LayerMask EnemyLayer;
	[SerializeField] private LayerMask GroundLayer;		
	
	private Rigidbody2D rb;
	private RaycastHit2D isCheckGround;	
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
	}
	
	private void Update()
	{
		if (hp <= 0) WhenDie();
		Pb.BarValue = hp*(100/FullHP); //Корректровка HP Bar
		
		//Движение:
		if (NotDie && !isSit && Input.GetButton("Horizontal")) Run();		  
		if (NotDie && isGround && Input.GetButtonDown("Jump")) Jump();
		if (NotDie && isGround && Input.GetKey(KeyCode.S)) {Sit(); isSit = true;} else isSit = false;
		if (NotDie && Input.GetButtonDown("Fire1")) Attack();		

		//Анимация:		
		if (NotDie && isGround && Input.GetButton("Horizontal")) anim.SetBool("isRun", true); else anim.SetBool("isRun", false);
		if (NotDie && !isGround) anim.SetBool("isJump", true); else anim.SetBool("isJump", false);
		if (NotDie && isGround && Input.GetKey(KeyCode.S)) anim.SetBool("isSit", true); else anim.SetBool("isSit", false);					
	}
	
	//Основные функции
	
	private void Run()
	{
		naprX = Input.GetAxis("Horizontal");
		Vector3 dir = transform.right*naprX;
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
			Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer);
			for (int i = 0; i<enemies.Length; i++)
			{
				enemies[i].GetComponent<FireGolem>().GetDamage(damageHero);
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
		isCheckGround = Physics2D.Raycast(transform.position, -Vector2.up, RayDistToGround, GroundLayer);					
		isGround = isCheckGround;						
	}
	
	public void GetDamage(int damageFireGolem)
	{
		hp -= damageFireGolem;					
	}		
	
	private void WhenDie()
	{
		anim.SetTrigger("isDie");
		NotDie = false;
		Invoke("Die",3);
		Invoke("ReloadLevel",3);
	}	


	//Функции интерфейса

	private void ReloadLevel()  //Respawn
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);				
	}

	private void OnDrawGizmosSelected() //Сфера для радиуса атаки
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);	
	}
	
}

			
	


