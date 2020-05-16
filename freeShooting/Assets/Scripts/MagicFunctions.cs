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
    [Header("Magic:Destroy Tower")]
    public GameObject destroyMissile;
    [Header("Magic:Destroy All Towers")]
    public GameObject Explosion;
    private GameObject ExplosionInstatiated;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    
    public void destroyAll(int player)
    {
        ExplosionInstatiated= Instantiate(Explosion, new Vector3(0, 12, 0), Explosion.transform.rotation);
        StartCoroutine(destoryAllTowers(ExplosionInstatiated));
    }
    IEnumerator destoryAllTowers(GameObject go)
    {
        for(int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(Time.deltaTime*10);
            go.transform.localScale += new Vector3(11, 10,20);
            
        }
        short Pcoins = GameManagerPartie.instance.playerCoins;
        short Ecoins = GameManagerPartie.instance.enemyCoins;
        for (int k = 0; k < 2; k++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[k, j] != null)
                {
                    Destroy(positionManager.buildingGameObject[k, j]);
                    positionManager.delete(positionManager.buildingGameObject[k, j].transform.position);

                }
            }

        }
        GameManagerPartie.instance.playerCoins = Pcoins;
        GameManagerPartie.instance.enemyCoins = Ecoins;
        GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
        GameManagerPartie.instance.enemyCoinsTxt.text = GameManagerPartie.instance.enemyCoins.ToString();
        GameManagerPartie.instance.ChangeSprites();
        Destroy(go);


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
                        Instantiate(destroyMissile, new Vector3(positionManager.buildingGameObject[1, j].transform.position.x, positionManager.buildingGameObject[1, j].transform.position.y + 50, positionManager.buildingGameObject[1, j].transform.position.z), destroyMissile.transform.rotation);
                        StartCoroutine(DestroyTower(positionManager.buildingGameObject[1, j]));
                        
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
    IEnumerator DestroyTower(GameObject go)
    {
        yield return new WaitForSeconds(2.2f);
        Destroy(go);

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
