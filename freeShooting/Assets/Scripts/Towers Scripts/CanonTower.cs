using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : MonoBehaviour
{
    float nextActionTime;
    public float period = 2.0f;
    private Vector3  target= new Vector3(-3,3,33);
    public GameObject CanonFireBall;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        nextActionTime = Time.time;
        //transform.LookAt(target);

       Vector3 relativePos = target - transform.position;
       Quaternion rotObject = Quaternion.LookRotation(relativePos, Vector3.up);
         rotObject = Quaternion.Euler(rotObject.eulerAngles.x, rotObject.eulerAngles.y, rotObject.eulerAngles.z);
         transform.rotation = rotObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time > nextActionTime) 
        {

            GameObject go = Instantiate(CanonFireBall,new Vector3(transform.position.x, transform.position.y, transform.position.z+3),transform.rotation);
            //go.transform.LookAt(target);
            rb = go.GetComponent<Rigidbody>();
            rb.velocity = go.transform.forward * 50;
            nextActionTime += period;
            



        }
    }
}
