using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShooting : MonoBehaviour
{
    //public float destroy= 2f;
    //public float range = 35f;
    public short damage = 10;
    
    public float fireRate = 4;
    private float nextTimeFire = 0f;
    public short speed = 25;
    private GameObject bullet_;
    private Transform firePoint;

    public void SetDamage(short d)
    {
        damage = d;
    }
    public void SetFireRate(float f)
    {
        fireRate = f;
    }

    private void Start()
    {
        firePoint = transform.GetChild(0);
        bullet_ = GameManager.instance.getGun().GunBullet;
    }
    //public ParticleSystem muzzleFlash;
    //public AudioSource[] hitSound = new AudioSource[4];

    //private bool enemyRightSide;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTimeFire)
        {
            shoot();
            nextTimeFire = Time.time + 1 / fireRate;
        }
    }

    private void shoot()
    {
       
        GameObject clone = Instantiate(bullet_, firePoint.position, firePoint.rotation);
        //clone.GetComponent<Rigidbody>().velocity = transform.localRotation.eulerAngles;
        //clone.transform.Rotation = transform.localRotation.eulerAngles;
        clone.GetComponent<Rigidbody>().velocity = transform.forward * speed; 
        clone.GetComponent<bullet>().changedam(damage);
        clone.GetComponent<bullet>().sender = gameObject;
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