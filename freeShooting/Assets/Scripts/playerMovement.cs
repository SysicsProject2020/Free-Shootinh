using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 5F;
    bool move = false;
    Vector3 destination;
    //public Transform PlayerZone;

    public void SetHealth(short h)
    {
        GetComponent<target>().Sethealth(h);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //need to be changing for each player 
                if (hit.collider.name == "PlayerZone")
                {
                    destination = new Vector3(hit.point.x, transform.position.y, transform.position.z);
                    move = true;
                }
            }
        }
        if (move)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
            if (transform.position == destination)
            {
                move = false;
            }
        }
    }
}

