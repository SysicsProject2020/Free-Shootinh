using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixHealthOrientation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.z > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
