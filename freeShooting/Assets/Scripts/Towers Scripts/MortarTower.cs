using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTower : MonoBehaviour
{
    public GameObject fireBall;
    private GameObject target = GameManagerPartie.enemy_;
    
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
       
        if (target.activeSelf)
        {
            if (Time.time > nextTimeFire)
            {
                shoot();
                nextTimeFire = Time.time + 1;
            }
        }

    }

    void shoot()
    {
        Vector3 FireballPos = new Vector3(target.transform.position.x, target.transform.position.y+3, target.transform.position.z);
        var heading = FireballPos - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        speed = (distance - transform.position.y + 10);
        

        GameObject go = Instantiate(fireBall, transform.position, this.transform.rotation);
        go.GetComponent<MortarFireBall>().Set(transform.position, direction, speed);

    }
}
