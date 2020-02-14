using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTower : MonoBehaviour
{
    public GameObject fireBall;
    private Vector3 target=new Vector3(-3,3,33);
    private Vector3 fireBallDirection;
    private float speed;

    public float fireRate = 1;
    private float nextTimeFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTimeFire)
        {
            shoot();
            nextTimeFire = Time.time + 1 / fireRate;
        }

    }

    void shoot()
    {
        var heading = target - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        speed = (distance - transform.position.y + 10);
        //fireBallDirection =   (target -transform.position);

        GameObject go = Instantiate(fireBall, transform.position, this.transform.rotation);
        go.GetComponent<MortarFireBall>().Set(transform.position, direction, speed);

    }
}
