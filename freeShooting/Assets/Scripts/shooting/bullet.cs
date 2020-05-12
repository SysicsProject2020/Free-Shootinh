using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullet : MonoBehaviour
{
    public short damage;
    public GameObject sender; 

    public void changedam(short dam)
    {
        damage = dam;
    }
    private void Awake()
    {
        if (transform.position.z < 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Player Bullet");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy Bullet");
        }          
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<target>() != null)
        {       
            if (sender != null)
            {
                if (collision.transform.GetComponent<mirrorTower>() != null)
                {
                    sender.transform.root.GetComponent<target>().takeDamage(damage);
                }
                else
                {
                    //magic
                    if (sender.transform.root.name == GameManagerPartie.instance.player_.name)
                    {
                        GameManagerPartie.instance.playerDamage += damage;
                        if (GameManagerPartie.instance.playerDamage >= 1000)
                        {
                            GameManagerPartie.instance.playerMagic1.GetComponent<Button>().interactable = true;
                            /*
                             GameManagerPartie.instance.playerMagic1.GetComponent<Button>().interactable = false;
                             GameManagerPartie.instance.playerDamage = 0;
                            */
                        }
                    }
                }         
            }
            collision.transform.GetComponent<target>().takeDamage(damage);

            //hitSound[choose].Play();           
        }
        Destroy(gameObject);
    }
}

