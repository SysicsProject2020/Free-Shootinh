using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazerShooting : MonoBehaviour
{

    public short damage;
    private short damageMultiplier = 2;
    public float fireRate = 1;
    private float nextTimeFire = 0f;

    Transform firePoint;

    LineRenderer lineRenderer;

    private GameObject target_;
    public TowerScript tower;


    private void Start()
    {
        firePoint = transform.GetChild(0);
        lineRenderer = firePoint.GetComponent<LineRenderer>();
        damage = GetComponent<towerInf>().damage;
    }

    private lazerState CurrentState = lazerState.idle;

    public enum lazerState
    {
        idle, shoot , finishShooting
            //, die
    }

    public void shoot(GameObject targetGameObject)
    {
        CurrentState = lazerState.shoot;
        target_ = targetGameObject;
    }
    public void stopShoot()
    {

        CurrentState = lazerState.finishShooting;

    }
    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case lazerState.idle:
                lineRenderer.enabled = false;
                target_ = null;
                break;

            case lazerState.finishShooting:
                damage = GetComponent<towerInf>().damage;
                lineRenderer.enabled = false;
                target_ = null;
                CurrentState = lazerState.idle;
                break;

            case lazerState.shoot:

                if (Time.time > nextTimeFire)
                {
                    lazer();
                    nextTimeFire = Time.time + fireRate;
                    damage *= damageMultiplier;
                } 
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