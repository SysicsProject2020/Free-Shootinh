using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingHealingTower : MonoBehaviour
{
    [SerializeField]
    byte degreesPerSecond = 55;  

void Update()
    {
        transform.Rotate(Vector3.up * degreesPerSecond * Time.deltaTime, Space.Self);
    }
}
