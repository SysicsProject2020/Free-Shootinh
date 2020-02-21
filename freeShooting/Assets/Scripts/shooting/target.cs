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
    private float healthOffsetZPlayer = -3.5f;
    private float healthOffsetZEnemy = 5f;
    private float healthOffsetZPlayerTower = -3.5f;
    private float healthOffsetZEnemyTower = 5f;
    public byte respawnTime = 3;
    private Vector3 respawnPoint;


    private void Start()
    {
        healthBarInst();
        respawnPoint = transform.position;       
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
        
        if (gameObject.GetComponent<AIeasy>() != null)
        {
            AIeasy.changeState(AIeasy.AIState.hide);
        }
    }
    public void gainhealth(short gain)
    {
        health += gain;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        healthBar.fillAmount = (float)health / (float)maxHealth;

    }
    void die()
    {
        if (gameObject.GetComponent<playerMovement>() != null)
        {
            gameObject.SetActive(false);
            transform.position = respawnPoint;
            health = maxHealth;
            Invoke("respawn", respawnTime);
        }
        else
        {
            positionManager.delete(transform.position);
            Destroy(gameObject);
            
        }
        GameManagerPartie.instance.startCoins += 50;
        GameManagerPartie.instance.startCoinsTxt.text = GameManagerPartie.instance.startCoins.ToString();
        GameManagerPartie.instance.ChangeSprites();
    }

    void respawn()
    {
        gameObject.SetActive(true);
    }


    void healthBarInst()
    {
        Vector3 pos;
        if (transform.position.z > 0)//enemy
        {
            if (gameObject.GetComponent<playerMovement>() == null)
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
            if (gameObject.GetComponent<playerMovement>() == null)
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
