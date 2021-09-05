using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinnBlast : MonoBehaviour
{
    [SerializeField] private int damage;          
    private float lifeTime;         
    private Transform player;   
    private Rigidbody2D rb;
    private Animator anim;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        lifeTime = 1f; 
        rb.AddForce(transform.right*25, ForceMode2D.Impulse);                 
    }

    private void Update()   
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) {anim.SetTrigger("isDie"); Destroy(gameObject, 0.5f);}
    }     
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Hero>().GetDamage(damage);
            anim.SetTrigger("isDie");
            Destroy(gameObject, 0.5f);            
        }         
    }
}
