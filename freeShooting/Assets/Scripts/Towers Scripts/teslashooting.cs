using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teslashooting : MonoBehaviour
{

    public short damage = 50;
    public float fireRate = 1;
    private float nextTimeFire = 0f;
    Transform firePoint;
    bool shooting = true;
    LineRenderer lineRenderer;


    GameObject target_;

    public TowerScript tower;

    void SetDamage()
    {
        damage = tower.Get_damage();
        this.GetComponent<target>().Sethealth(tower.Get_health());

    }
    private void Start()
    {
        firePoint = transform.GetChild(0);
        lineRenderer = firePoint.GetChild(0).GetComponent<LineRenderer>();
        SetDamage();
    }

    public teslaState CurrentState = teslaState.idle;

    public enum teslaState
    {
        idle, shoot, finishShooting
        //, die
    }
 
    public void shoot(GameObject targetGameObject)
    {
        CurrentState = teslaState.shoot;
        target_ = targetGameObject;
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
                    nextTimeFire = Time.time + fireRate;
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
        target_.GetComponent<target>().takeDamage(damage);
    }
}