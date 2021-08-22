using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // public int Damage(int damage)
    // {
    //     return damage;
    // }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }
}
