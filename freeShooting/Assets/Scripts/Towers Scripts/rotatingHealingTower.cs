using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingHealingTower : MonoBehaviour
{
    Rigidbody rb;
    float degreesPerSecond = 50.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    

void Update()
    {
        transform.Rotate(Vector3.up*degreesPerSecond*Time.deltaTime, Space.Self);
        //transform.Rotate(Vector3.left * degreesPerSecond * Time.deltaTime, Space.Self);

        rb.isKinematic = true;

    }
}
