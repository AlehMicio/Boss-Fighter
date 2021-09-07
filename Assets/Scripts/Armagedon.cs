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

    [HideInInspector] public bool armagedon;
	private float cdFire;
    
	
	private void Start()
	{
		armagedon = false;
		cdFire = 0;
	}
	
	private void Update()
	{
		if (armagedon)
		{
			Fire1();
			Fire2();
			Fire3();
			Fire4();
			Fire5();
			Fire6();
			Fire7();
			Fire8();
			Fire9();	
		}
	}
	
	
	private void Fire1()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point1.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}
	
	private void Fire2()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point2.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}

	private void Fire3()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point3.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}

	private void Fire4()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point4.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}
	
	private void Fire5()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point5.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}
	
	private void Fire6()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point6.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}

	private void Fire7()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point7.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}

	private void Fire8()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point8.position, point1.rotation);
			cdFire = 0.3f;
		}
		else cdFire -= Time.deltaTime;	
	}

	private void Fire9()
	{
		if (cdFire <= 0)
		{
			Instantiate(armagedonBlast, point9.position, point1.rotation);
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
