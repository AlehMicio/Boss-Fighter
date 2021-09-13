using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBlast : Entity
{
    [SerializeField] private int damage;         
    private float lifeTime;

    private Rigidbody2D rb;
    private SpriteRenderer spriteBlast;
    private bool player;    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteBlast = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>().flipHero;        
    }
    private void Start()
    {
        lifeTime = 0.5f;
        if (player == false) rb.AddForce(transform.right*25, ForceMode2D.Impulse);
         else {spriteBlast.flipX = true; rb.AddForce(-transform.right*25, ForceMode2D.Impulse);}    
    }

    private void Update()   
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(gameObject);                          
    }     
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Entity>().Damage(damage);
            Destroy(gameObject);
        }         
    }
}
