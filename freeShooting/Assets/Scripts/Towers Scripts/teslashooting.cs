using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teslashooting : MonoBehaviour
{
    private float nextTimeFire = 0f;
    public Transform firePoint;
    bool shooting = true;
    LineRenderer lineRenderer;
    GameObject target_;

    private void Start()
    {      
        lineRenderer = firePoint.GetComponent<LineRenderer>();
    }

    public teslaState CurrentState = teslaState.idle;

    public enum teslaState
    {
        idle, shoot, finishShooting, shootMirror
        //, die
    }
 
    public void shoot(GameObject targetGameObject)
    {
        target_ = targetGameObject;
        if (target_.GetComponent<mirrorTower>() != null)
            CurrentState = teslaState.shootMirror;
        else
            CurrentState = teslaState.shoot;
    }
    public void StopShoot()
    {
        CurrentState = teslaState.finishShooting;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case teslaState.idle:
                break;

            case teslaState.finishShooting:
                lineRenderer.enabled = false;
                target_ = null;
                CurrentState = teslaState.idle;
                break;

            case teslaState.shoot:

                if (Time.time > nextTimeFire)
                {
                    /*****************************************************/
                    //need rethink
                    if (shooting)
                    {
                        tesla();
                    }
                    else
                    {
                        lineRenderer.enabled = false;
                    }
                    nextTimeFire = Time.time + gameObject.GetComponent<towerInf>().fireRate;
                    shooting = !shooting;
                }
                break;
            case teslaState.shootMirror:

                if (Time.time > nextTimeFire)
                {
                    /*****************************************************/
                    //need rethink
                    if (shooting)
                    {
                        tesla();
                        GetComponent<target>().takeDamage(GetComponent<towerInf>().damage);
                    }
                    else
                    {
                        lineRenderer.enabled = false;
                    }
                    nextTimeFire = Time.time + (1 / gameObject.GetComponent<towerInf>().fireRate);
                    shooting = !shooting;
                }
                break;
        }

    }
    
    void tesla()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target_.transform.position);       
        target_.GetComponent<target>().takeDamage(GetComponent<towerInf>().damage);
    }
}