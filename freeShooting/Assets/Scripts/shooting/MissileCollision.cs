using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollision : MonoBehaviour
{
    
    private void Start()
    {
         GetComponent<Rigidbody>().velocity = new Vector3(0, -10, 0);

    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
