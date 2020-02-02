using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    public float health = 100f;

    private void Start()
    {
    }
    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            die();
        }
    }
    void die()
    {
        //deathSound.deathS();
        Destroy(gameObject, 0.5f);
    }
}
