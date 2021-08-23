using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
	private Vector3 pos;
	
	private void Start()
	{
		if (!player) player = FindObjectOfType<Hero>().transform; 
	}
	
	private void Update()
	{
		pos = player.position;
		pos.z = -10f;
		pos.y = player.position.y + 2f;
		transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);			
	}
}
