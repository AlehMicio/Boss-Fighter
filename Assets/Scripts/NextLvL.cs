using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLvL : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D Next)
	{
		if (Next.transform.tag == "Player") NextLevel();				
	}
	
    private void NextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("Level 2");		
	}
}
