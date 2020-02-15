﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    bool enemyRightSide;
    public short damage;
    private Plane p;
    public void changedam(short dam)
    {
        damage = dam;
    }
    private void Start()
    {
        if (transform.position.z < 0)
            enemyRightSide = true;
        else
            enemyRightSide = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "stadium")
        {
            Destroy(gameObject);
            return;
        }

        if (collision.transform.tag == "bullet")
        {
            Destroy(gameObject);
            return;
        }
            

        if (collision.transform.position.z > 0 == enemyRightSide)
        {
             collision.transform.GetComponent<target>().takeDamage(damage);
             //hitSound[choose].Play();            
        }
        Destroy(gameObject);

    }
   
}

