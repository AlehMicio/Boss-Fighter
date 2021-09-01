using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : Entity
{
    [SerializeField] private Hero hero;

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
		    hero.GetComponent<Hero>().hp += 20;						
		}
		Die();	
	}


}
