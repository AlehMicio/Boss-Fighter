using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointForRazgon : Entity
{
    [SerializeField] private EarthGolem Golem; 
   
    private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			Golem.GetComponent<EarthGolem>().razgon = true;
			Die();									
		}				
	}

	public void RespawnRazgonPoint()
    {
        this.gameObject.SetActive(true);				
    }
}
