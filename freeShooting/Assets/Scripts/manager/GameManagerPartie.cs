using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPartie : MonoBehaviour
{
    public TowerScript towerBase;
    public TowerScript enemybase;
    public PlayerScript player;
    public PlayerScript enemy;
    public GameObject itemParent;
    private Vector3 playerPos = new Vector3(5, 1.8f, -25);
    private Vector3 enemypos = new Vector3(5, 1.8f, 25);
    private Vector3 playerTowerPos = new Vector3(5, 2.2f, -38);
    private Vector3 enemyTowerPos = new Vector3(5, 2.2f, 38);
    private TowerScript[] towersSelected = new TowerScript[6];

    void Start()
    {
        towersSelected = GameManager.instance.GetSelectedTowers();
        towerBase.prefab.GetComponent<target>().health = towerBase.Get_health();
        enemybase.prefab.GetComponent<target>().health = enemybase.Get_health();
        player.prefab.GetComponent<gun>().damage = player.Get_damage();
        enemy.prefab.GetComponent<gun>().damage = enemy.Get_damage();
        ChangeSprites();
        instantiatePrefabs();
       
    }
    private void instantiatePrefabs()
    {
        Instantiate(towerBase.prefab, playerTowerPos, Quaternion.Euler(0, 0, 0));
        Instantiate(enemybase.prefab, enemyTowerPos, Quaternion.Euler(-180, 0, 0));

        Instantiate(player.prefab, playerPos, Quaternion.Euler(0, 0, 0));
        Instantiate(enemy.prefab, enemypos, Quaternion.Euler(-180, 0, 0));
    }
    private void ChangeSprites()
    {
        for (int i = 0; i < 6; i++)
        {
            itemParent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = towersSelected[i].image;
        }
    }
}
