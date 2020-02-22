using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazerShooting : MonoBehaviour
{

    public short damage = 5;
    short damageInit ;
    private short damageMultiplier = 2;
    public float fireRate = 1;
    private float nextTimeFire = 0f;

    Transform firePoint;

    LineRenderer lineRenderer;

    public GameObject target_;

    private void Start()
    {
        damageInit = damage;
        firePoint = transform.GetChild(0);
        lineRenderer = firePoint.GetComponent<LineRenderer>();
    }



    public lazerState CurrentState = lazerState.idle;

    public enum lazerState
    {
        idle, shoot, finishShoot
            //, die
    }
    /*public static void changeState(lazerState state)
    {
        CurrentState = state;
        //Debug.Log(state);
    }*/


    public void shoot(GameObject targetGameObject)
    {
        CurrentState = lazerState.shoot;
        target_ = targetGameObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case lazerState.idle:
                break;

            case lazerState.shoot:

                if (target_ != null)
                {
                    if (Time.time > nextTimeFire)
                    {
                        lazer();
                        nextTimeFire = Time.time + fireRate;
                        damage *= damageMultiplier;
                    }
                }
                else
                {
                    CurrentState = lazerState.finishShoot;
                } 
                break;

            case lazerState.finishShoot:

                damage = damageInit;
                lineRenderer.enabled = false;
                CurrentState = lazerState.idle;
                break;
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