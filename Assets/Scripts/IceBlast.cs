using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlast : Entity
{
    [SerializeField] private int damage;
    private float speed = 20;

    private Transform player;
    private Rigidbody2D rb;
 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed*Time.deltaTime);
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
