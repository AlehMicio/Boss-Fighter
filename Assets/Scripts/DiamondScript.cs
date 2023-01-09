using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondScript : Entity
{
   [SerializeField] private Text txt;
   private int point;

   private void Start()
   {
	   point = PlayerPrefs.GetInt("PD", point);
	   txt.text = point.ToString();
   }	
   
   private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			point += 1;
			PlayerPrefs.SetInt("PD", point);
			PlayerPrefs.Save();
			txt.text = point.ToString();
			//PlayerPrefs.DeleteKey("PD");			
		}
		Die();	
	}
}
