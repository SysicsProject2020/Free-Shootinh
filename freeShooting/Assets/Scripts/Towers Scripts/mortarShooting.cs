﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortarShooting : MonoBehaviour
{
    public GameObject fireBall;
    private GameObject target = GameManagerPartie.enemy_;
    private Vector3 tar = new Vector3(-3,3,33);
    private float speed;

    public float fireRate = 1;
    private float nextTimeFire = 0f;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        /* if (target.activeSelf)
         {

         }*/
        if (Time.time > nextTimeFire)
        {
            Vector3 vo = Calculatevelocity(tar,transform.position, 5);
            shoot(vo);
            nextTimeFire = Time.time + 3;
        }

    }

    void shoot(Vector3 vo)
    {
        /*Vector3 FireballPos = new Vector3(target.transform.position.x, target.transform.position.y + 3, target.transform.position.z);
        var heading = FireballPos - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        speed = (distance - transform.position.y + 10);*/

        
        GameObject go = Instantiate(fireBall, transform.position,transform.rotation);
        go.GetComponent<Rigidbody>().velocity = vo;

        //go.GetComponent<MortarFireBall>().Set(transform.position, direction, speed);

    }
    Vector3 Calculatevelocity(Vector3 target,Vector3 origin,float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;
        float Sy = distanceXZ.y;
        float sXZ = distanceXZ.magnitude;
        float Vxz = sXZ / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;
        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;

    }
}