using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private int hp;
	public ProgressBar Pb;
	private float FullHP;

 	private void Start()
	{
		FullHP = hp;
	}
	
	private void Update()
	{
		if (hp <= 0) Die();
		Pb.BarValue = hp*(100/FullHP);
	}

	private void OnCollisionEnter2D(Collision2D enemy)
	{
		if (enemy.gameObject == Hero.Instance.gameObject)
		{ 
			Hero.Instance.GetDamage();			
		}		
	}
	public void GetDamage1(int damage1)
	{
		hp -= damage1;
	}
	
}
