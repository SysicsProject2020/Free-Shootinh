using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MagicFunctions : MonoBehaviour
{
    public static MagicFunctions instance;
    [Header("Magic:see Cards")]
    public Canvas canvas;
    public GameObject towerPanel;
    public GameObject TowerPanelInstantiated;
    
    [Header("Magic:Shield")]
    public GameObject shield;
    private GameObject ShieldInstantiated;
    [Header("Magic:AllCardsFree")]
    private bool testFree=false;
    private short currentCoins;
    [Header("Magic:MissilesMagic")]
    public GameObject Missiles;
    private GameObject MissilesInstantiated;
    private bool MissilesLaunched=false;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    
    public void destroyAll(int player)
    {
        short Pcoins=GameManagerPartie.instance.playerCoins;
        short Ecoins = GameManagerPartie.instance.enemyCoins;
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[i,j] != null)
                {
                    Destroy(positionManager.buildingGameObject[i, j]);
                    positionManager.delete(positionManager.buildingGameObject[i, j].transform.position);

                }
            }

        }
        GameManagerPartie.instance.playerCoins = Pcoins;
        GameManagerPartie.instance.enemyCoins = Ecoins;
        GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
        GameManagerPartie.instance.enemyCoinsTxt.text = GameManagerPartie.instance.enemyCoins.ToString();
        GameManagerPartie.instance.ChangeSprites();

    }
    public void Shield(int player)
    {
        if (player == 0)
        {
            ShieldInstantiated=Instantiate(shield, GameManagerPartie.instance.playerTowerPos, Quaternion.Euler(0, 0, 0));
            Invoke("removeShield", 10);
        }
        else
        {
            Instantiate(shield, GameManagerPartie.instance.enemyTowerPos, Quaternion.Euler(0, 0, 0));
        }
    }
    void removeShield()
    {
        Destroy(ShieldInstantiated);
    }
    public void destroyEnemyTower(int player)
    {
        if (player == 0)
        {
            int i = 0;
            int k = 0;
            short Pcoins = GameManagerPartie.instance.playerCoins;
            
                for (int j = 0; j < 5; j++)
                {
                    if (positionManager.buildingGameObject[1, j] != null)
                    {
                    i++;
                    }
                }
            int rand = Random.Range(0, i-1);
            
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[1, j] != null)
                {
                    
                    if (k == rand)
                    {
                        Destroy(positionManager.buildingGameObject[1, j]);
                        positionManager.delete(positionManager.buildingGameObject[1, j].transform.position);
                    }
                    k++;
                }
            }
            GameManagerPartie.instance.playerCoins = Pcoins;
            GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
            GameManagerPartie.instance.ChangeSprites();

        }
        else
        {
            
        }
    }
    public void Accelerate(int player)
    {
        if (player == 0)
        {
           
        }
        else
        {
           
        }
    }
    public void Clone(int player)
    {
        if (player == 0)
        {

        }
        else
        {

        }
    }
    public void FreezTower(int player)
    {
        if (player == 0)
        {

        }
        else
        {

        }
    }
    public void MissilesMagic(int player)
    {
        MissilesInstantiated = Instantiate(Missiles);
        MissilesLaunched = true;
        
    }
    void hideCards(int player) {
        Destroy(TowerPanelInstantiated);
    }
    public void allCardsFree(int player)
    {
        if (player == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                GameManagerPartie.instance.itemParent.transform.GetChild(i).GetComponent<Button>().interactable = true;
                GameManagerPartie.instance.itemParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "0";
            }
            testFree = true;
            currentCoins = GameManagerPartie.instance.playerCoins;
        }
        else
        {

        }
    }
    void returnCardsValues(int player)
    {
        GameManagerPartie.instance.ChangeSprites();
        testFree = false;
    }
    // Update is called once per frame
    public void healTowers(int player)
    {
        if (player == 0)
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {
                    positionManager.buildingGameObject[0, j].gameObject.GetComponent<target>().gainhealth(150);
                }
               
            }
            GameManagerPartie.instance.player_.GetComponent<target>().gainhealth(50);
            GameManagerPartie.instance.playerTowerBase_.GetComponent<target>().gainhealth(200);
        }
        else
        {

        }
    }

    void Update()
    {
        if (testFree)
        {
            if (currentCoins > GameManagerPartie.instance.playerCoins)
            {
                GameManagerPartie.instance.playerCoins = currentCoins;
                GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
                returnCardsValues(0);
            }
            else
                currentCoins = GameManagerPartie.instance.playerCoins;
        }
        if (MissilesLaunched)
        {
            if (MissilesInstantiated.transform.childCount ==0)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (positionManager.buildingGameObject[1, j] != null)
                    {
                        positionManager.buildingGameObject[1, j].GetComponent<target>().takeDamage(75);
                    }
                }
                GameManagerPartie.instance.enemy_.GetComponent<target>().takeDamage(50);
                GameManagerPartie.instance.enemyTowerBase_.GetComponent<target>().takeDamage(75);
                Destroy(MissilesInstantiated);
                MissilesLaunched = false;
            }
        }
    }
}
/*public void SeeCards(int player)
{
    if (player == 0)
    {
        TowerPanelInstantiated = Instantiate(towerPanel,GameManagerPartie.instance.itemParent.transform.position,Quaternion.Euler(0,0,0),canvas.transform);
        for (int i = 0; i < 6; i++)
        {
            if (AIeasy.towers[i].cost > GameManagerPartie.instance.enemyCoins)
            {
                towerPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
                //change sprite to non interactable
                //itemParent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = towersSelected[i].image;
            }
            else
            {
                towerPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
                //change sprite to interactable
                towerPanel.transform.GetChild(i).GetComponentInChildren<Image>().sprite = AIeasy.towers[i].image;
            }
            towerPanel.transform.GetChild(i).GetComponentInChildren<Text>().text = AIeasy.towers[i].cost.ToString();
        }
        Invoke("hideCards", 2.0f);
    }
    else
    {

    }
}*/
