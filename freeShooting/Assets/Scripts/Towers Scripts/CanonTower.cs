using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : MonoBehaviour
{
    float nextTimeFire;
    GameObject target_;
    public GameObject CanonFireBall;
    public Transform firePoint;
    public Transform rotationPart;
    public byte speed = 19;
    //private GameObject head;

    

    // Start is called before the first frame update
    void Start()
    {
        nextTimeFire = Time.time;;
    }
    private canonState CurrentState = canonState.idle;

    public enum canonState
    {
        idle, shoot
        //, die
    }

    public void shoot(GameObject targetGameObject)
    {
        CurrentState = canonState.shoot;
        target_ = targetGameObject;
        rotation();
    }
    public void stopShoot()
    {
        CurrentState = canonState.idle;
        target_ = null;
    }
    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case canonState.idle:
                break;

            case canonState.shoot:
                if (Time.time > nextTimeFire)
                {
                    if (target_ != null)
                    {
                        GameObject go = Instantiate(CanonFireBall, firePoint.position, firePoint.rotation);
                        go.GetComponent<Rigidbody>().velocity = go.transform.forward * speed;
                        go.GetComponent<bullet>().changedam(GetComponent<towerInf>().damage);
                        go.GetComponent<bullet>().sender = gameObject;
                        go.GetComponent<canonFireBall>().pos(transform.position, target_.transform.position);
                        nextTimeFire = Time.time + gameObject.GetComponent<towerInf>().fireRate;
                    }
                    
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
}
