using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xbowShooting : MonoBehaviour
{
    //public float destroy= 2f;
    //public float range = 35f;
    public short damage = 10;

    public float fireRate = 4;
    private float nextTimeFire = 0f;
    
    public short speed = 25;
    public GameObject bow;
    public Transform firePoint;
    public Transform rotationPart;
    GameObject target_;

    private void Start()
    {
        target_ = GameManagerPartie.enemy_;
    }

    // Update is called once per frame
    void Update()
    {
        if (target_.activeSelf)
        {
            if (Time.time > nextTimeFire)
            {
                shoot();
                nextTimeFire = Time.time + 1;
            }
        }
    }

    private void shoot()
    {
        Vector3 relativePos = target_.transform.position - rotationPart.position;
        Quaternion rotObject = Quaternion.LookRotation(relativePos, Vector3.up);
        //rotObject = Quaternion.Euler(rotObject.eulerAngles.x, rotObject.eulerAngles.y, rotObject.eulerAngles.z);
        rotationPart.transform.rotation = rotObject;


        GameObject clone = Instantiate(bow, firePoint.position, firePoint.rotation);
        //clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(target_.transform.position.x, target_.transform.position.y, target_.transform.position.z);
        clone.GetComponent<Rigidbody>().velocity = firePoint.transform.forward * speed;
        clone.GetComponent<bullet>().changedam(damage);
    }


}