using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    public short health = 100;

    private void Start()
    {
    }
    public void takeDamage(short damage)
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
        Destroy(gameObject);
    }
}
