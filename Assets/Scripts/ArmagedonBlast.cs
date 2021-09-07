using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmagedonBlast : MonoBehaviour
{
    [SerializeField] private int damage;         
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
        rotZ = -135;               
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
