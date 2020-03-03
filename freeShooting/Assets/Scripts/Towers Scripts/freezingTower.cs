﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezingTower : MonoBehaviour
{
    Transform firePoint;
    LineRenderer lineRenderer;
    public GameObject target_;
    float targetFireRate;

    private void Start()
    {
        firePoint = transform.GetChild(0);
        lineRenderer = firePoint.GetChild(0).GetComponent<LineRenderer>();
    }

    public freezingtowerState CurrentState = freezingtowerState.idle;

    public enum freezingtowerState
    {
        idle, shoot
        //, die
    }

    public void shoot(GameObject targetGameObject)
    {
        target_ = targetGameObject;
        if (target_.GetComponent<mirrorTower>() == null)
            CurrentState = freezingtowerState.shoot;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case freezingtowerState.idle:
                break;

            case freezingtowerState.shoot:
                freeze();
                break;
        }

    }

    void freeze()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target_.transform.position);
        targetFireRate = target_.GetComponent<towerInf>().fireRate;
        target_.GetComponent<towerInf>().fireRate = (targetFireRate / 100) * gameObject.GetComponent<towerInf>().damage;
        CurrentState = freezingtowerState.idle;
    }

    public void unfreeze()
    {
        target_.GetComponent<towerInf>().fireRate = targetFireRate;
        lineRenderer.enabled = false;
        target_ = null;
        CurrentState = freezingtowerState.idle;
    }
}
