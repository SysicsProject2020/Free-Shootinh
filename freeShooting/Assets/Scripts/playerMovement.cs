using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 5F;
    public Transform target;
    bool move = false;
    Vector3 destination;
    //public Transform PlayerZone;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "PlayerZone")
                {
                    destination = hit.point;
                    destination.y = transform.position.y;
                    destination.z = transform.position.z;

                    move = true;
                }
            }
        }

        if (move)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
            transform.LookAt(target);
            if (transform.position == destination)
            {
                move = false;
            }
        }
    }
}

