using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingMissiles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }




    void Update()
    {
        transform.Rotate(new Vector3(0.8f, 0, 0));
    }
    // Update is called once per frame


}
