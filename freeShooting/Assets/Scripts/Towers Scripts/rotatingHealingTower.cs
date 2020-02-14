using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingHealingTower : MonoBehaviour
{
    public float degreesPerSecond = 50.0f;

    

void Update()
    {
        transform.Rotate(Vector3.up*degreesPerSecond*Time.deltaTime, Space.Self);


    }
}
