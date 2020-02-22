using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teslashooting : MonoBehaviour
{

    public short damage = 50;
    public float fireRate = 1;
    private float nextTimeFire = 0f;
    Transform firePoint;
    bool shoot = true;
    LineRenderer lineRenderer;


    GameObject target_;

    private void Start()
    {
        target_ = GameManagerPartie.enemy_;
        firePoint = transform.GetChild(0);
        lineRenderer = firePoint.GetChild(0).GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (target_ == null)
        {
            if (GameManagerPartie.enemy_ != null)
            {
                target_ = GameManagerPartie.enemy_;
            }
            else
            {
                return;
            }

        }
        //fire condition
        if (target_.activeSelf)
        {
            if (Time.time > nextTimeFire)
            {
                if (shoot)
                {
                    tesla();
                }
                else
                {
                    lineRenderer.enabled = false;
                }          
                nextTimeFire = Time.time + fireRate;
                shoot = !shoot;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void tesla()
    {
        lineRenderer.enabled = true;
        /*
        Vector3 direction = (target_.transform.position - firePoint.transform.position).normalized;
        float distance = Vector3.Distance(firePoint.transform.position , target_.transform.position);

        Vector3[] lines = new Vector3[5];
        lines[0] = firePoint.position;
        lines[4] = target_.transform.position;
        Debug.Log(direction);
        Debug.Log(distance);
        Debug.Log(lines[1]);
        Debug.DrawLine(lines[0], lines[1], Color.red, Mathf.Infinity);
        for (int i = 0; i < 5; i++)
        {
            lineRenderer1.SetPosition(i, lines[i]);
        }
        */
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target_.transform.position);
        
        target_.GetComponent<target>().takeDamage(damage);
    }
}