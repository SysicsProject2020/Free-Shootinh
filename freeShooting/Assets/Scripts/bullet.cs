using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    bool enemyRightSide;
    public short damage;
    public void changedam(short dam)
    {
        damage = dam;
    }
    private void Start()
    {
        if (transform.position.z < 0)
            enemyRightSide = true;
        else
            enemyRightSide = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.position.z > 0 == enemyRightSide)
        {
            /**********************************/
            //test tower , base and player collider
            if (collision.transform.tag !="stadium")
            {
                collision.transform.GetComponent<target>().takeDamage(damage);
                //hitSound[choose].Play();
            }
        }
        Destroy(gameObject);

    }
    /*private void OnTriggerEnter(Collider other)
    {
        if(Collider.Equals(PlayerScript))
    }*/

}

