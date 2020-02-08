using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    //public float range = 35f;
    public short damage = 10;
    public float fireRate = 4;
    private float nextTimeFare = 0f;

    //public float destroy= 2f;
    public GameObject bullet;
    public short speed = 25;

    //public ParticleSystem muzzleFlash;
    //public AudioSource[] hitSound = new AudioSource[4];

    //private bool enemyRightSide;

    /*private void Start()
    {
        if (transform.position.z < 0)
            enemyRightSide = true;
        else
            enemyRightSide = false;
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTimeFare)
        {
            shoot();
            nextTimeFare = Time.time + 1 / fireRate;
        }
    }

    private void shoot()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2f);
        GameObject clone = Instantiate(bullet, pos, transform.rotation);
        clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(0, 0, speed);
        clone.GetComponent<bullet>().changedam(damage);


    }

    /*void shootRayCast()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * range, Color.red, 0f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.transform.position.z > 0 == enemyRightSide)
            {
                //Debug.Log(hit.transform.name);
                //test tower , base and player collider
                if (true)
                {
                    //muzzleFlash.Play();

                    hit.transform.GetComponent<target>().takeDamage(damage);

                    //GameObject impactBlood = Instantiate(blood, hit.point, Quaternion.LookRotation(hit.normal));
                    //Destroy(impactBlood, 0.5f);

                    //int choose = Random.Range(0, 3);

                    //hitSound[choose].Play();
                }


            }

        }
    }*/
}