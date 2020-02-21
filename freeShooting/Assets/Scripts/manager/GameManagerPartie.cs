using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPartie : MonoBehaviour
{



    public static GameManagerPartie instance;
    public short startCoins = 1000;
    public Text startCoinsTxt;

    [Header("Player 1")]
    public TowerScript towerBase;
    public PlayerScript player;
    public GameObject itemParent;
    public static Vector3 playerPos = new Vector3(5, 1.8f, -25);
    private Vector3 playerTowerPos = new Vector3(5, 2.2f, -38);
    public static TowerScript[] towersSelected = new TowerScript[6];
    public static GameObject player_;

    [Header("Player 2")]
    public TowerScript enemybase;
    public PlayerScript enemy;
    private Vector3 enemypos = new Vector3(5, 1.8f, 25);
    private Vector3 enemyTowerPos = new Vector3(5, 2.2f, 38);
    public static TowerScript[] EnemySelectedTowers = new TowerScript[6];
    public static GameObject enemy_;
    private void Awake()
    {

        instance = this;
    }
    void Start()
    {
        towersSelected = GameManager.instance.GetSelectedTowers();
        towerBase.prefab.GetComponent<target>().health = towerBase.Get_health();
        enemybase.prefab.GetComponent<target>().health = enemybase.Get_health();
        player.prefab.GetComponent<playerShooting>().damage = player.Get_damage();
        enemy.prefab.GetComponent<playerShooting>().damage = enemy.Get_damage();
        ChangeSprites();
        instantiatePrefabs();

        chooseEnemyTowers();
        startCoinsTxt.text = startCoins.ToString();
    }
    private void instantiatePrefabs()
    {
        Instantiate(towerBase.prefab, playerTowerPos, Quaternion.Euler(0, 0, 0));
        Instantiate(enemybase.prefab, enemyTowerPos, Quaternion.Euler(-180, 0, 0));

        player_ = Instantiate(player.prefab, playerPos, Quaternion.Euler(0, 0, 0));
        enemy_ = Instantiate(enemy.prefab, enemypos, Quaternion.Euler(-180, 0, 0));
    }
    public void ChangeSprites()
    {
        for (int i = 0; i < 6; i++)
        {
            if (towersSelected[i].cost > startCoins)
            {
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
            itemParent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = towersSelected[i].image;
            itemParent.transform.GetChild(i).GetComponentInChildren<Text>().text = towersSelected[i].cost.ToString();
        }
    }
    void chooseEnemyTowers()
    {
        EnemySelectedTowers = towersSelected;

    }
}
