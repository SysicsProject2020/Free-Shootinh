using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 5F;
    bool move = false;
    Vector3 destination;
    Animator anim;
    float distance = 1F;
    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        anim.SetFloat("x", 0.5f);
       //GameManagerPartie.instance.player_.GetComponent<Animator>().
    }
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
                if (hit.transform.tag == "heroTouchCollider") 
                {
                    if (hit.point.x < GameManagerPartie.instance.player_.transform.position.x)
                    {

                        anim.SetFloat("x", 0);

                    }
                    else
                        anim.SetFloat("x", 1);

                    destination = new Vector3(hit.point.x, transform.position.y, transform.position.z);
                    distance = Mathf.Abs(hit.point.x - transform.position.x);
                    move = true;
                }
                
            }
        }
        if (move)
        {
            //Debug.Log(distance);
            transform.position = Vector3.Lerp(transform.position, destination, 36/distance * speed * Time.deltaTime);
            if (Mathf.Abs(destination.x - transform.position.x) < 1f)
            {               
                move = false;
                anim.SetFloat("x", 0.5f);
            }
        }
    }
}

