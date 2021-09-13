using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hero : Entity
{	
	public ProgressBar Pb;
	public float hp;	
	public Transform CheckPoint;			
			
	private float HPtxt;
	private float naprX;		
	private int damageHero1 = 1;
	private int damageHero2 = 3;
	private float AttackRange = 0.8f;
	private float jumpForce = 7;
	private float speed;
	private float cdFire;	
	[HideInInspector] public float FullHP;	
	
	private bool isGround;
	private bool isRoof;		
	private bool NotDie = true;			
	private bool cd1; 
	private bool cd2;
	[HideInInspector] public bool flipHero;	

	[SerializeField] private Text txt;
	[SerializeField] private Transform AttackPoint1;
	[SerializeField] private Transform AttackPoint2;	
	[SerializeField] private LayerMask EnemyLayer;
	[SerializeField] private LayerMask GroundLayer;
	[SerializeField] private CapsuleCollider2D capsul;
	[SerializeField] private CircleCollider2D circle;
	[SerializeField] private GameObject heroBlast; 			
	
	private Rigidbody2D rb;
	private RaycastHit2D isCheckGround;
	private RaycastHit2D isCheckRoof;	
	private Animator anim;	
	private Transform enemy;	
	private SpriteRenderer sprite;	
	
	//Программные функции
 	
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponent<Animator>();
		enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
	}
	
	
	private void Start()
	{
		cd1 = false;
		cd2 = false;
		cdFire = 0;
		FullHP = hp;											
	}
	
	private void FixedUpdate()
	{
		CheckGround();
		CheckRoof();								
	}
	
	private void Update()
	{
		if (hp <= 0) WhenDie();
		Pb.BarValue = hp*(100/FullHP); //Корректровка HP Bar
		if (hp <= 0) HPtxt = 0; else HPtxt = hp;
		txt.text = "HP " + HPtxt; 
		
		//Движение:		
		Run();
		Jump();
		Sit();
		Attack1();
		Attack2();
		Fire();

		if (cdFire > 0) cdFire -= Time.deltaTime;						
	}
	
	//Основные функции
	
	private void Run()
	{
		if (NotDie && Input.GetButton("Horizontal"))
		{
			naprX = Input.GetAxis("Horizontal");
			Vector3 dir = transform.right*naprX;
			transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed*Time.deltaTime);					
			sprite.flipX = dir.x < 0.0f; flipHero = dir.x < 0.0f;		
		}

		if (NotDie && isGround && Input.GetButton("Horizontal")) anim.SetBool("isRun", true);
		 else anim.SetBool("isRun", false);

	}
	
	private void Jump()
	{
		if (NotDie && isGround && Input.GetButtonDown("Jump"))	rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);		
		if (NotDie && !isGround) anim.SetBool("isJump", true);
		 else anim.SetBool("isJump", false);
	}
	
	private void Sit()  
	{
		if (NotDie && isGround && Input.GetKey(KeyCode.S))
		{
			speed = 1;
			anim.SetBool("isSit", true);
			capsul.enabled = false;
			circle.enabled = true; 
		}
		else 
		{
			speed = 5;
			anim.SetBool("isSit", false);
			capsul.enabled = true;
			circle.enabled = false;
		}
	}

	private void Attack1()
	{
		if (NotDie && !cd1 && Input.GetButtonDown("Fire1"))
		{
			anim.SetTrigger("isAttack1");
			if (sprite.flipX == false)		
			{
				Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint1.position, AttackRange, EnemyLayer);			 
				for (int i = 0; i<enemies.Length; i++)
				{
					enemies[i].GetComponent<Entity>().Damage(damageHero1);
				}
			}
			else
			{
				Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint2.position, AttackRange, EnemyLayer);			 
				for (int i = 0; i<enemies.Length; i++)
				{
					enemies[i].GetComponent<Entity>().Damage(damageHero1);
				}
			}
			cd1 = true;			
			StartCoroutine(AttackCoolDown1());
		}
	}

	private void Attack2()
	{
		if (NotDie && !cd2 && Input.GetButtonDown("Fire2"))
		{
			anim.SetTrigger("isAttack2");		
			if (sprite.flipX == false)		
			{
				rb.AddForce(transform.right*3, ForceMode2D.Impulse);								
			}
			else
			{
				rb.AddForce(-transform.right*3, ForceMode2D.Impulse);
			}			
			cd2 = true;			
			StartCoroutine(AttackCoolDown2());
		}
	}

	private void OnAttack2()
	{
		if (sprite.flipX == false)		
		{
			Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint1.position, AttackRange, EnemyLayer);			 
			for (int i = 0; i<enemies.Length; i++)
			{
				enemies[i].GetComponent<Entity>().Damage(damageHero2);
			}				
		}
		else
		{
			Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint2.position, AttackRange, EnemyLayer);			 
			for (int i = 0; i<enemies.Length; i++)
			{
				enemies[i].GetComponent<Entity>().Damage(damageHero2);
			}					
		}			
	}	

	private  IEnumerator AttackCoolDown1()
	{
		yield return new WaitForSeconds(0.5f);
		cd1 = false;
	}

	private  IEnumerator AttackCoolDown2()
	{
		yield return new WaitForSeconds(3f);
		cd2 = false;
	}

	private void Fire()
	{
		if (cdFire <= 0 && Input.GetButtonDown("Fire3"))
		{
			Instantiate(heroBlast, transform.position, transform.rotation);
			cdFire = 5f;
		}			
	}

	//Вспомогательные функции

	private void CheckGround()
	{
		isCheckGround = Physics2D.Raycast(transform.position, -Vector2.up, 1f, GroundLayer);					
		isGround = isCheckGround;						
	}

	private void CheckRoof()
	{
		isCheckRoof = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, GroundLayer);					
		isRoof = isCheckRoof;						
	}
	
	public void GetDamage(float damageEnemy)
	{
		hp -= damageEnemy;					
	}		

	public void Otdacha(float damageEnemy)
	{
		if (enemy.position.x > transform.position.x) rb.AddForce(-transform.right*8, ForceMode2D.Impulse);
		else rb.AddForce(transform.right*8, ForceMode2D.Impulse);
		hp -= damageEnemy;		
	}
	
	private void WhenDie()
	{
		anim.SetTrigger("isDie");
		NotDie = false;
		Invoke("Die", 3);
		Invoke("ReloadLevel",3);
		//Invoke("Respawn", 3.1f);		
	}	

	private void Respawn()
   {
        this.gameObject.SetActive(true);		
		if (hp <= 0) hp = FullHP;		
		transform.position = CheckPoint.position;						
		NotDie = true;
   }

	//Функции интерфейса

	private void ReloadLevel()  
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);						
	}		

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Border") {GetDamage(5); Die(); Invoke("Respawn", 3.1f);}
	}
		
}

			
	


