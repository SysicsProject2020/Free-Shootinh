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
    public byte PlayerRespawnTime = 3;

    public void Sethealth(short health)
    {
        this.health = health;
        maxHealth = health;
        //Debug.Log(health + "    "+  maxHealth);
    }
    void Awake()
    {
        healthBarInst();      
    }
    public void takeDamage(short damage)
    {

        health -= damage;
        if (transform.position.z < 0)
        {
            GameManagerPartie.instance.enemyDamage += damage;
            if (GameManagerPartie.instance.enemyDamage >= 1500)
            {
                AIeasy.changeState(AIeasy.AIState.Magic1);
            }
        }
        else
        {
            GameManagerPartie.instance.playerTotalDamage += (uint)damage;
            GameManagerPartie.instance.playerDamage += damage;
            if (GameManagerPartie.instance.playerDamage >= 1500)
            {
                GameManagerPartie.instance.SetMagic1Enable();
            }
        }
        if (health <= 0)
        {
            die();
            return;
        }
        //Debug.Log((float)health / (float)maxHealth);
        healthBar.fillAmount = (float)health / (float)maxHealth;
        
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

        //player kills
        /*GameManagerPartie.instance.playerKills++;
        if (GameManagerPartie.instance.playerKills >= 5)
        {
            GameManagerPartie.instance.playerMagic2.GetComponent<Button>().interactable = true;
        }*/
        if (gameObject.GetComponent<playerMovement>() != null)
        {
            gameObject.SetActive(false);
            health = maxHealth;
            healthBar.fillAmount = (float)health / (float)maxHealth;

            if (transform.position.z < 0)
            {
                Vibrator.Vibrate();
                GameManagerPartie.instance.enemyCoins += 50;
                GameManagerPartie.instance.enemyCoinsTxt.text = GameManagerPartie.instance.enemyCoins.ToString();
                transform.position = GameManagerPartie.instance.playerPos;
            }
            else
            {
                GameManagerPartie.instance.playerCoins += 50;
                GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
                transform.position = GameManagerPartie.instance.enemyPos;
            }
            //GameManagerPartie.instance.ChangeSprites();
            Invoke("respawn", PlayerRespawnTime);
        }
        else
        {
            if (gameObject.GetComponent<towerBase>() != null)
            {
                if (transform.position.z < 0)
                {
                    GameManagerPartie.instance.lose();
                }
                else
                {
                    GameManagerPartie.instance.win();
                }
                Destroy(gameObject);
            }
            else
            {
                if (transform.position.z < 0)
                {
                    GameManagerPartie.instance.enemyKills++;
                    if (GameManagerPartie.instance.enemyKills >= 5)
                    {
                        AIeasy.changeState(AIeasy.AIState.Magic2);
                    }
                }
                else
                {
                    GameManagerPartie.instance.playerKills++;
                    if (GameManagerPartie.instance.playerKills >= 5)
                    {
                        GameManagerPartie.instance.SetMagic2Enable();
                    }
                }
                if (gameObject.GetComponent<freezingTower>() != null)
                {
                    gameObject.GetComponent<freezingTower>().reverse();
                }
                Debug.Log(transform.position + "   " + gameObject.name);
                Destroy(gameObject);
                positionManager.delete(transform.position);

            }
        }       
    }

    void respawn()
    {
        gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<Animator>().SetFloat("x", 0.5f);
    }

        //nbadel na3mel lel player public methode w lel towers public methode an9es if(s)
    void healthBarInst()
    {
        Vector3 pos;
        if (transform.position.z > 0)//enemy
        {
            if (gameObject.GetComponent<playerMovement>() != null)
            {
                
                pos = new Vector3(transform.position.x, GameManager.instance.healthOffsetYplayers, transform.position.z + GameManager.instance.healthOffsetZEnemy);
            }
            else
            {
                if (gameObject.GetComponent<towerBase>() != null)
                {
                    pos = new Vector3(transform.position.x, transform.GetChild(0).position.y + GameManager.instance.healthOffsetYEnemyBase, transform.position.z + GameManager.instance.healthOffsetZEnemyTowerBase);
                }
                else
                {
                    pos = new Vector3(transform.position.x, transform.GetChild(0).position.y + GameManager.instance.healthOffsetY, transform.position.z + GameManager.instance.healthOffsetZEnemyTower);
                }
                
            }

        }
        else
        {
            if (gameObject.GetComponent<playerMovement>() != null)
            {               
                pos = new Vector3(transform.position.x, GameManager.instance.healthOffsetYplayers, transform.position.z + GameManager.instance.healthOffsetZPlayer);
            }
            else
            {
                if (gameObject.GetComponent<towerBase>() != null)
                {
                    pos = new Vector3(transform.position.x, transform.GetChild(0).position.y + GameManager.instance.healthOffsetY, transform.position.z + GameManager.instance.healthOffsetZPlayerTowerBase);
                }
                else
                {
                    pos = new Vector3(transform.position.x, transform.GetChild(0).position.y + GameManager.instance.healthOffsetY, transform.position.z + GameManager.instance.healthOffsetZPlayerTower);
                }
                
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
