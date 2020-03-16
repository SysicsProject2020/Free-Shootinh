using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPartie : MonoBehaviour
{
    public static GameManagerPartie instance;

    [Header("Player 1: player")]
    public TowerScript towerBase;
    private PlayerScript player;
    public GameObject itemParent;
    public Vector3 playerPos = new Vector3(25, 1.8f, -25);
    public Vector3 playerTowerPos = new Vector3(5, 2.2f, -38);
    public TowerScript[] towersSelected = new TowerScript[6];
    public GameObject player_;
    public GameObject playerGun_;
    public GunsScript playerGun;
    public GameObject playerTowerBase_;
    public short playerCoins = 1000;
    public Text playerCoinsTxt;
    public GameObject playerMagic1;
    public GameObject playerMagic2;
    public short playerDamage=0;
    public byte playerKills = 0;

    [Header("Player 2: enemy")]
    public TowerScript enemybase;
    public PlayerScript enemy;
    public Vector3 enemyPos = new Vector3(-15, 1.8f, 25);
    public Vector3 enemyTowerPos = new Vector3(5, 2.2f, 38);
    public TowerScript[] EnemySelectedTowers = new TowerScript[6];
    public GameObject enemy_;
    public GameObject enemyGun_;
    public GunsScript enemyGun;
    public GameObject enemyTowerBase_;
    public short enemyCoins = 10000;
    public Text enemyCoinsTxt;
    [Range(1,5)]
    public byte enemylvl;

    private void Awake()
    {
        instance = this;
        
    }
    void Start()
    {
        
        player = GameManager.instance.getPlayer();
        //Debug.Log(player.name);
        setMagic();
        towersSelected = GameManager.instance.GetSelectedTowers();

        ChangeSprites();
        instantiatePrefabs();
        chooseEnemyTowers();

        playerCoinsTxt.text = playerCoins.ToString();
        enemyCoinsTxt.text = enemyCoins.ToString();
        
    }
    private void setMagic()
    {
        
        switch (player.name)
        {
            case "robot":
                /*playerMagic1.GetComponent<Image>().sprite = player.magic1.image;
                playerMagic2.GetComponent<Image>().sprite = player.magic2.image;*/
                playerMagic1.GetComponent<Button>().onClick.AddListener(() => { MagicFunctions.instance.Accelerate(0); });
                playerMagic2.GetComponent<Button>().onClick.AddListener(() => { MagicFunctions.instance.Shield(0); });
                break;
            case "shadow":
                playerMagic1.GetComponent<Button>().onClick.AddListener(() => { MagicFunctions.instance.healTowers(0); });
                playerMagic2.GetComponent<Button>().onClick.AddListener(() => { MagicFunctions.instance.allCardsFree(0); });
                break;
            case "snowman":
                playerMagic1.GetComponent<Button>().onClick.AddListener(() => { MagicFunctions.instance.MissilesMagic(0); });
                playerMagic2.GetComponent<Button>().onClick.AddListener(() => { MagicFunctions.instance.FreezTower(0); });                
                break;
            case "witcher":
                
                playerMagic1.GetComponent<Button>().onClick.AddListener(() => { MagicFunctions.instance.destroyEnemyTower(0); });
                playerMagic2.GetComponent<Button>().onClick.AddListener(() => { MagicFunctions.instance.destroyAll(0); });

                break;
        }
        playerMagic1.GetComponent<Button>().interactable = false;
        playerMagic2.GetComponent<Button>().interactable = false;
    }

    private void instantiatePrefabs()
    {
        playerGun = GameManager.instance.getGun();
        enemyGun = GameManager.instance.getGun();

        playerTowerBase_= Instantiate(towerBase.prefab, playerTowerPos, Quaternion.Euler(0, 0, 0));
        changeLayerMask(playerTowerBase_, "Player");
        playerTowerBase_.GetComponent<towerInf>().SetHealth(towerBase.Get_health_player());
        enemyTowerBase_ =Instantiate(enemybase.prefab, enemyTowerPos, Quaternion.Euler(180, 0, 0));
        changeLayerMask(enemyTowerBase_, "Enemy");
        enemyTowerBase_.GetComponent<towerInf>().SetHealth(towerBase.Get_health_enemy(enemylvl));

        player_ = Instantiate(player.prefab, playerPos, Quaternion.Euler(0, 0, 0));
        enemy_ = Instantiate(enemy.prefab, enemyPos, Quaternion.Euler(180, 0, 0));
        player_.GetComponent<PlayerMovementWithSliderBar>().SetHealth(player.Get_health_player());
        enemy_.GetComponent<PlayerMovementWithSliderBar>().SetHealth(enemy.Get_health_enemy(enemylvl));
        changeLayerMask(enemy_, "Enemy");
        changeLayerMask(player_, "Player");

        playerGun_ = Instantiate(playerGun.prefab, playerPos, Quaternion.Euler(0, 0, 0),player_.transform.GetChild(0));
        enemyGun_ = Instantiate(enemyGun.prefab, enemyPos, Quaternion.Euler(180, 0, 0),enemy_.transform.GetChild(0));
        playerGun_.GetComponent<playerShooting>().SetDamage(20);
        playerGun_.GetComponent<playerShooting>().SetFireRate(playerGun.Get_fireRate_Gun_player());
        enemyGun_.GetComponent<playerShooting>().SetDamage(playerGun.Get_damage_Gun_enemy(enemylvl));
        enemyGun_.GetComponent<playerShooting>().SetFireRate(playerGun.Get_fireRate_Gun_enemy(enemylvl));
    }
    public void ChangeInteractableSprites()
    {
        for (int i = 0; i < 6; i++)
        {
            if (towersSelected[i].cost > playerCoins)
            {
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = false;
                //change sprite to non interactable
                //itemParent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = towersSelected[i].image;
            }
            else
            {
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = true;
                //change sprite to interactable
                itemParent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = towersSelected[i].image;
            }
        }
    }

    public void ChangeSprites()
    {
        for (int i = 0; i < 6; i++)
        {
            if (towersSelected[i].cost > playerCoins)
            {
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = false;
                //change sprite to non interactable
                //itemParent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = towersSelected[i].image;
            }
            else
            {
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = true;
                //change sprite to interactable
                itemParent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = towersSelected[i].image;
            }
            itemParent.transform.GetChild(i).GetComponentInChildren<Text>().text = towersSelected[i].cost.ToString();
        }
    }

    //public AIeasy AI;
    void chooseEnemyTowers()
    {
        /*
        EnemySelectedTowers = towersSelected;
        AI.towers = EnemySelectedTowers;
        
       for (int i = 1; i < 6; i++)
        {
            if (EnemySelectedTowers[i].cost < AI.minCostTower)
            {
                AI.minCostTower = (byte)i;
            }
        }*/
    }


    private void changeLayerMask(GameObject go, string layer)
    {
        go.layer = LayerMask.NameToLayer(layer);
        foreach (Transform g in go.transform)
        {
            g.gameObject.layer = LayerMask.NameToLayer(layer);
            foreach (Transform gobj in g.transform)
            {
                gobj.gameObject.layer = LayerMask.NameToLayer(layer);
            }
        }
    }
}
