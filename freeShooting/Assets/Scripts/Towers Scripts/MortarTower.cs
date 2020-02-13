using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTower : MonoBehaviour
{
    public GameObject fireBall;
    private Vector3 target=new Vector3(-3,3,33);
    private Vector3 fireBallDirection;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
        var heading = target - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        speed = (distance - transform.position.y+10);
        //fireBallDirection =   (target -transform.position);

        GameObject go = Instantiate(fireBall,transform.position, this.transform.rotation);
  
        go.GetComponent<MortarFireBall>().Set(transform.position, direction, speed);
    }

    // Update is called once per frame
    void Update()
    {
       
              
         
    }


}
