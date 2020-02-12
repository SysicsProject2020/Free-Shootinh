using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class target : MonoBehaviour
{
    public short health = 100;
    private short maxHealth;
    private Image healthBar;
    public GameObject healthBarInstatiate;
    private float healthOffsetY = 3;
    private float healthOffsetZPlayer = -3.7f;
    private float healthOffsetZEnemy = 2f;


    private void Start()
    {
        Vector3 pos;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        if (transform.position.z > 0)//enemy
        {
            pos = new Vector3(transform.position.x, transform.position.y + healthOffsetY, transform.position.z + healthOffsetZEnemy);
        }
        else
        {
            pos = new Vector3(transform.position.x, transform.position.y + healthOffsetY, transform.position.z + healthOffsetZPlayer);
        }
        GameObject obj = Instantiate(healthBarInstatiate, pos, rot, transform);    
        Vector3 relativePos = Camera.main.transform.position - obj.transform.position;
        Quaternion rotCam = Quaternion.LookRotation(relativePos, Vector3.up);
        rotCam = Quaternion.Euler(rotCam.eulerAngles.x, 180, 0);
        obj.transform.rotation = rotCam;

        healthBar = obj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        maxHealth = health;
    }
    public void takeDamage(short damage)
    {
        health -= damage;

        if (health <= 0)
        {
            die();

        }
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }
    void die()
    {
        //deathSound.deathS();
        Destroy(gameObject);
    }
}
