using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{other.GetComponent<Hero>().CheckPoint.position = other.transform.position;
		Debug.Log("Check Point!");}
	}   
 }
