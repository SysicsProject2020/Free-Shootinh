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
    public Vector3 healthOffset;

    private void Start()
    {
        GameObject obj = Instantiate(healthBarInstatiate, transform.position+ healthOffset, transform.rotation,transform);
        obj.transform.LookAt(Camera.main.transform);
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
