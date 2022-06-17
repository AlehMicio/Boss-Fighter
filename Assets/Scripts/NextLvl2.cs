using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLvl2 : MonoBehaviour
{
    [SerializeField] private BoxCollider2D BoxCol;
    [SerializeField] private LayerMask Enemy;
   
    void Update()
    {
        if (Physics2D.Raycast(transform.position, -Vector2.right, 100, Enemy) == true) BoxCol.enabled = false;
         else  BoxCol.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D Next)
	{
		if (Next.gameObject.tag == "Player") NextLevel();				
	}
	
    private void NextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        		
	}
}
