using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointForRazgon : Entity
{
    [SerializeField] private EarthGolem raz; 
   
    private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			raz.GetComponent<EarthGolem>().razgon = true;
			Debug.Log("Razgon!");			
		}
		Die();
		Invoke("Respawn", 6);
	}

	private void Respawn()
   {
        this.gameObject.SetActive(true);				
   }
}
