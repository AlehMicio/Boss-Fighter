using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armagedon : Entity
{
    [SerializeField] private GameObject armagedonBlast;
	[SerializeField] private Transform point1;
	[SerializeField] private Transform point2;
	[SerializeField] private Transform point3;
	[SerializeField] private Transform point4;
	[SerializeField] private Transform point5;
	[SerializeField] private Transform point6;
	[SerializeField] private Transform point7;
	[SerializeField] private Transform point8;
	[SerializeField] private Transform point9;
	private  Transform[] arrayPoints;

    [HideInInspector] public bool armagedon;
	private float cdFire;
    
	
	private void Start()
	{
		armagedon = false;
		cdFire = 0;
		arrayPoints = new Transform[9] {point1, point2, point3, point4, point5, point6, point7, point8, point9};
	}
	
	private void Update()
	{
		if (armagedon)
		{
			for (int i = 0; i<9; i++){
				Fire(arrayPoints[i]);
			}
		}
	}	
	
	private void Fire(Transform point){
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}	
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			armagedon = true;												
		}				
	}	
}
