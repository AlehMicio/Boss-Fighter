using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : Entity
{
    [SerializeField] private Hero health;

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
		    health.GetComponent<Hero>().hp += 5;						
		}
		Die();	
	}


}
