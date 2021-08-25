using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public virtual void Damage(int damage)
    {
       
    }

    public void Die()
    {
        this.gameObject.SetActive(false);                
    }   
}
