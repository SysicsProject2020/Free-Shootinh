using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xbowShooting : MonoBehaviour
{
    private float nextTimeFire = 0f;
    
    public short speed = 25;
    public GameObject bow;
    public Transform firePoint;
    public Transform rotationPart;
    GameObject target_;


    private xbowState CurrentState = xbowState.idle;

    public enum xbowState
    {
        idle, shoot
        //, die
    }



    public void shoot(GameObject targetGameObject)
    {
        CurrentState = xbowState.shoot;
        target_ = targetGameObject;
        rotation();
    }
    public void stopShoot()
    {
        CurrentState = xbowState.idle;
        target_ = null;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case xbowState.idle:
                break;

            case xbowState.shoot:
                if (Time.time > nextTimeFire)
                {
                    shoot();
                    nextTimeFire = Time.time + gameObject.GetComponent<towerInf>().fireRate;
                }
                break;
        }
        
    }

    void rotation()
    {
        Vector3 relativePos = target_.transform.position - rotationPart.position;
        Quaternion rotObject = Quaternion.LookRotation(relativePos, Vector3.up);
        rotObject = Quaternion.Euler(transform.rotation.x, rotObject.eulerAngles.y, transform.rotation.z);
        rotationPart.rotation = rotObject;
    }
    private void shoot()
    {
        GameObject clone = Instantiate(bow, firePoint.position, firePoint.rotation);
        //clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(target_.transform.position.x, target_.transform.position.y, target_.transform.position.z);
        clone.GetComponent<Rigidbody>().velocity = firePoint.transform.forward * speed;
        clone.GetComponent<bullet>().changedam(gameObject.GetComponent<towerInf>().damage);
        clone.GetComponent<bullet>().sender = gameObject;
    }


}