using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateModel : MonoBehaviour
{
    public float rotationSpeed = 80f;
    void OnMouseDrag()
    {
        float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
        transform.RotateAround(transform.up, -x);
    }
}
