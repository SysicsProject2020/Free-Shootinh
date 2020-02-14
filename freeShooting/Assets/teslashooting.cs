using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teslashooting : MonoBehaviour
{

    public short damage = 5;
    public short damageInit;
    private short damageMultiplier = 2;

    private float nextTimeFire = 0f;
    Transform firePoint;

    LineRenderer lineRenderer1;
    LineRenderer lineRenderer2;


    GameObject target_;

    private void Start()
    {
        damageInit = damage;
        target_ = GameManagerPartie.enemy_;
        firePoint = transform.GetChild(0);
        lineRenderer1 = firePoint.GetChild(0).GetComponent<LineRenderer>();
        lineRenderer2 = firePoint.GetChild(1).GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        //fire condition
        if (target_.activeSelf)
        {
            if (Time.time > nextTimeFire)
            {
                lazer();
                nextTimeFire = Time.time + 1;
                damage *= damageMultiplier;
            }
        }
        else
        {
            damage = damageInit;
            lineRenderer1.enabled = false;
            lineRenderer2.enabled = false;

        }
    }

    void lazer()
    {
        lineRenderer1.enabled = true;
        lineRenderer2.enabled = true;



        /*lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target_.transform.position);
        target_.GetComponent<target>().takeDamage(damage);*/
    }
}
