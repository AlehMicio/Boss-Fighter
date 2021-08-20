using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseBorder : MonoBehaviour
{

	private void OnCollisionEnter2D(Collision2D FinishBorder)
	{
		if (FinishBorder.transform.tag == "Player") ReloadLevel();
		if (FinishBorder.transform.tag == "Enemy") ReloadLevel();		
	}
	
    private void ReloadLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);				
		
	}
}
