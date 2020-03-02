using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class target : MonoBehaviour
{
    public short health;
    private short maxHealth;
    private Image healthBar;
    public GameObject healthBarInstatiate;
    private float healthOffsetY = 3;
    private float healthOffsetZPlayer = -3.5f;
    private float healthOffsetZEnemy = 5f;
    private float healthOffsetZPlayerTower = -3.5f;
    private float healthOffsetZEnemyTower = 5f;
    public byte PlayerRespawnTime = 3;

    public void Sethealth(short health)
    {
        this.health = health;
        maxHealth = health;
    }

    private void Start()
    {
        healthBarInst();      
    }
    public void takeDamage(short damage)
    {

        health -= damage;

        if (health <= 0)
        {
            die();
            return;
        }
        healthBar.fillAmount = (float)health / (float)maxHealth;
        //Debug.Log((float)health / (float)maxHealth);
    }
    public void gainhealth(short gain)
    {
        health += gain;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        healthBar.fillAmount = (float)health / (float)maxHealth;
    }
    void die()
    {
        if (gameObject.GetComponent<playerShooting>() != null)
        {
            
            gameObject.SetActive(false);
            health = maxHealth;
            healthBar.fillAmount = (float)health / (float)maxHealth;

            if (transform.position.z < 0)
            {
                GameManagerPartie.instance.enemyCoins += 50;
                GameManagerPartie.instance.enemyCoinsTxt.text = GameManagerPartie.instance.enemyCoins.ToString();
                transform.position = GameManagerPartie.playerPos;
            }
            else
            {
                GameManagerPartie.instance.playerCoins += 50;
                GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
                transform.position = GameManagerPartie.enemyPos;
            }

            GameManagerPartie.instance.ChangeSprites();

            Invoke("respawn", PlayerRespawnTime);
        }
        else
        {
            if (gameObject.GetComponent<freezingTower>() != null)
            {
                gameObject.GetComponent<freezingTower>().unfreeze();
            }
            positionManager.delete(transform.position);
            Destroy(gameObject);           
        }       
    }

    void respawn()
    {
        gameObject.SetActive(true);
    }

    //nbadel na3mel lel player public methode w lel towers public methode an9es if(s)
    void healthBarInst()
    {
        Vector3 pos;
        if (transform.position.z > 0)//enemy
        {
            if (gameObject.GetComponent<playerShooting>() == null)
            {
                pos = new Vector3(transform.position.x, transform.position.y + healthOffsetY, transform.position.z + healthOffsetZEnemyTower);
            }
            else
            {
                pos = new Vector3(transform.position.x, transform.position.y + healthOffsetY, transform.position.z + healthOffsetZEnemy);
            }

        }
        else
        {
            if (gameObject.GetComponent<playerShooting>() == null)
            {
                pos = new Vector3(transform.position.x, transform.position.y + healthOffsetY, transform.position.z + healthOffsetZPlayerTower);
            }
            else
            {
                pos = new Vector3(transform.position.x, transform.position.y + healthOffsetY, transform.position.z + healthOffsetZPlayer);
            }

        }

        GameObject obj = Instantiate(healthBarInstatiate, pos, Quaternion.Euler(0, 0, 0), transform);
        //rotate health bar
        Vector3 relativePos = Camera.main.transform.position - obj.transform.position;
        Quaternion rotCam = Quaternion.LookRotation(relativePos, Vector3.up);
        rotCam = Quaternion.Euler(rotCam.eulerAngles.x, 180, 0);
        obj.transform.rotation = rotCam;

        healthBar = obj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
}
