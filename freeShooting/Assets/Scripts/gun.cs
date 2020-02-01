using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public float range = 35f;
    public float damage = 10f;
    public float fireRate = 10f;
    private float nextTimeFare =0f;

    //public ParticleSystem muzzleFlash;
    //public AudioSource[] hitSound = new AudioSource[4];

    private bool enemyRightSide;

    private void Start()
    {
        if (transform.position.z < 0)
            enemyRightSide = true;
        else
            enemyRightSide = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTimeFare)
        {
            shoot();
            nextTimeFare = Time.time + 1 / fireRate;
        }

    }

    void shoot()
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
    }
}
