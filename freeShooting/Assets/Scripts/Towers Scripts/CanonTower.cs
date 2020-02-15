using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : MonoBehaviour
{
    public short damage = 50;
    float nextTimeFire;
    public float fireRate = 2.0f;
    GameObject target_;
    public GameObject CanonFireBall;
    Transform firePoint;
    Transform rotationPart;
    public byte speed = 30;
    //private GameObject head;
    // Start is called before the first frame update
    void Start()
    {
        target_ = GameManagerPartie.enemy_;
        nextTimeFire = Time.time;
        firePoint = transform.GetChild(0).GetChild(0);
        rotationPart = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > nextTimeFire)
        {
            rotation();

            GameObject go = Instantiate(CanonFireBall,firePoint.position,firePoint.rotation);
            go.GetComponent<Rigidbody>().velocity = go.transform.forward * speed;
            go.GetComponent<bullet>().changedam(damage);
            go.GetComponent<canonFireBall>().pos(transform.position,target_.transform.position);
            nextTimeFire = Time.time + fireRate;
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
