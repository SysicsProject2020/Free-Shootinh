using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingHealingTower : MonoBehaviour
{
    [SerializeField]
    byte degreesPerSecond = 10;  

void Update()
    {
        transform.Rotate(Vector3.forward * degreesPerSecond * Time.deltaTime*5, Space.Self);
    }
}
