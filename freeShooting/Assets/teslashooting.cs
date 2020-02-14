using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teslashooting : MonoBehaviour
{

    public short damage = 50;

    private float nextTimeFire = 0f;
    Transform firePoint;

    LineRenderer lineRenderer1;
    LineRenderer lineRenderer2;


    GameObject target_;

    private void Start()
    {
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
                tesla();
                nextTimeFire = Time.time + 1;
            }
        }
        else
        {
            lineRenderer1.enabled = false;
            lineRenderer2.enabled = false;

        }
    }

    void tesla()
    {
        lineRenderer1.enabled = true;
        //lineRenderer2.enabled = true;
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
        lineRenderer1.SetPosition(0, firePoint.position);
        lineRenderer1.SetPosition(1, target_.transform.position);
        
        target_.GetComponent<target>().takeDamage(damage);
    }
}