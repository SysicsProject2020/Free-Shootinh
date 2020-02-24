using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    bool enemyRightSide;
    public short damage;
    private Plane p;
    public void changedam(short dam)
    {
        damage = dam;
    }
    private void Start()
    {
        if (transform.position.z < 0)
        {
            enemyRightSide = true;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        else
        {
            enemyRightSide = false;
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }          
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "stadium")
        {
            Destroy(gameObject);
            return;
        }

        if (collision.transform.tag == "bullet")
        {
            Destroy(gameObject);
            return;

        }
        if (collision.transform.GetComponent<target>() != null)
        {
            collision.transform.GetComponent<target>().takeDamage(damage);
            //hitSound[choose].Play();           
        }
        Destroy(gameObject);
    }
   /* private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "stadium")
        {
            Destroy(gameObject);
            return;
        }

        if (other.transform.tag == "bullet")
        {
            //Destroy(gameObject);
            return;

        }
        if (other.transform.GetComponent<target>() != null)
        {
            other.transform.GetComponent<target>().takeDamage(damage);
            //hitSound[choose].Play();           
        }
        Destroy(gameObject);
    }*/

}

