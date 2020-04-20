using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezingTower : MonoBehaviour
{
    public Transform firePoint;
    LineRenderer lineRenderer;
    public GameObject target_ = null;
    float targetFireRate;

    private void Awake()
    {
        lineRenderer = firePoint.GetComponent<LineRenderer>();
    }

    public freezingtowerState CurrentState = freezingtowerState.idle;

    public enum freezingtowerState
    {
        idle, shoot
        //, die
    }

    public void shoot(GameObject targetGameObject)
    {
        Debug.Log("freeze shoot");
        if (targetGameObject.GetComponent<mirrorTower>() == null)
        {
            target_ = targetGameObject;
            CurrentState = freezingtowerState.shoot;
        }
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
        target_ = null;
        lineRenderer.enabled = false;
        CurrentState = freezingtowerState.idle;
    }

    public void reverse()
    {
        if(target_!=null)
        target_.GetComponent<towerInf>().fireRate = targetFireRate;
    }
}
