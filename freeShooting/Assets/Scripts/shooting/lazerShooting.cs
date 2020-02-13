using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazerShooting : MonoBehaviour
{
    //public float destroy= 2f;
    //public float range = 35f;
    public short damage = 5;
    private short damageMultiplier = 2;
    
    private float nextTimeFire = 0f;
    //public short speed = 25;
    Transform firePoint;

    LineRenderer lineRenderer;

    GameObject target_;

    private void Start()
    {
        //target_ = GameManagerPartie.enemy_;
        firePoint = transform.GetChild(0);
        lineRenderer = firePoint.GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        //fire condition
        if (target_ != null)
        {
            if (Time.time > nextTimeFire)
            {
                lazer();
                nextTimeFire = Time.time + 1;
                Debug.Log(nextTimeFire);
                damage *= damageMultiplier;
                Debug.Log(damage);
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void lazer()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target_.transform.position);
        target_.GetComponent<target>().takeDamage(damage);
    }

}