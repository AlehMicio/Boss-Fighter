using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlast : MonoBehaviour
{
    [SerializeField] private int damage;    
    private float x;
    private float y;  
    private float lifeTime; 
    private float rotZ;      
    private Transform player;   
    private Rigidbody2D rb;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
       lifeTime = 1f; 

       if (player.position.x > transform.position.x)
          if (player.position.y > transform.position.y)
          {
            x = Mathf.Abs(player.position.x - transform.position.x);
            y = Mathf.Abs(player.position.y - transform.position.y);
            rotZ = Mathf.Atan2(y,x) * Mathf.Rad2Deg;
          }
          else
          {
            x = Mathf.Abs(player.position.x - transform.position.x);
            y = Mathf.Abs(transform.position.y - player.position.y);
            rotZ = -1*Mathf.Atan2(y,x) * Mathf.Rad2Deg;
          }
        else
          if (player.position.y > transform.position.y)
          {
            x = Mathf.Abs(transform.position.x - player.position.x);
            y = Mathf.Abs(player.position.y - transform.position.y);
            rotZ = -1*Mathf.Atan2(y,x) * Mathf.Rad2Deg + 180;
          }
          else
          {
            x = Mathf.Abs(transform.position.x - player.position.x);
            y = Mathf.Abs(transform.position.y - player.position.y);
            rotZ = Mathf.Atan2(y,x) * Mathf.Rad2Deg + 180;
          }
        
        transform.rotation = Quaternion.Euler(Vector3.forward * rotZ);
        rb.AddForce(transform.right*25, ForceMode2D.Impulse);                 
    }

    private void Update()   
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(gameObject);                          
    }     
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Hero>().GetDamage(damage);
            Destroy(gameObject);
        }         
    }
}
