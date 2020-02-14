using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : MonoBehaviour
{
    public short damage = 50;
    float nextActionTime;
    public float period = 2.0f;
    GameObject target_;
    public GameObject CanonFireBall;
    public Transform firePoint;
    public Transform rotationPart;
    public byte speed = 30;
    //private GameObject head;
    // Start is called before the first frame update
    void Start()
    {

        target_ = GameManagerPartie.enemy_;
        nextActionTime = Time.time;
        //head = gameObject.ge
        //transform.LookAt(target);
        firePoint = transform.GetChild(0).GetChild(0);
        rotationPart = transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time > nextActionTime) 
        {
            rotation();
    
            GameObject go = Instantiate(CanonFireBall,firePoint.position,firePoint.rotation);
            go.GetComponent<Rigidbody>().velocity = go.transform.forward * speed;
            go.GetComponent<bullet>().changedam(damage);
            nextActionTime += period;

        }
    }
    void rotation()
    {
        Vector3 relativePos = target_.transform.position - rotationPart.position;
        Quaternion rotObject = Quaternion.LookRotation(relativePos, Vector3.up);
        rotObject = Quaternion.Euler(rotationPart.rotation.x, rotObject.eulerAngles.y, rotationPart.rotation.z);
        rotationPart.rotation = rotObject;
    }
}
