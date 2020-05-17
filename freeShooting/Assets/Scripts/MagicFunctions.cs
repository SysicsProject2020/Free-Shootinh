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
    
    [Header("Magic:AllCardsFree")]
    private bool testFree=false;
    private short currentCoins;
    [Header("Magic:MissilesMagic")]
    public GameObject Missiles;
    private GameObject MissilesInstantiated;
    
    
    [Header("Magic:Destroy Tower")]
    public GameObject destroyMissile;
    [Header("Magic:Destroy All Towers")]
    public GameObject Explosion;
    private GameObject ExplosionInstatiated;
    [Header("Magic:DirectShot")]
    
    public GameObject DamageMissiles;
    public float speed;
    [Header("Magic:CarrotShield")]
    public GameObject Carrot;
    public GameObject CarrotWall;



    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    
    public void destroyAll()
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
            GameObject ShieldInstantiated=Instantiate(shield, GameManagerPartie.instance.playerTowerPos, Quaternion.Euler(0, 0, 0));
            StartCoroutine(removeShield(ShieldInstantiated));
        }
        else
        {
            GameObject ShieldInstantiated = Instantiate(shield, GameManagerPartie.instance.enemyTowerPos, Quaternion.Euler(0, 0, 0));
            StartCoroutine(removeShield(ShieldInstantiated));
        }
    }
    IEnumerator removeShield(GameObject go)
    {
        yield return new WaitForSeconds(10);
        Destroy(go);
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
            int i = 0;
            int k = 0;
            short Pcoins = GameManagerPartie.instance.enemyCoins;

            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {
                    i++;
                }
            }
            int rand = Random.Range(0, i - 1);

            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {

                    if (k == rand)
                    {
                        Instantiate(destroyMissile, new Vector3(positionManager.buildingGameObject[0, j].transform.position.x, positionManager.buildingGameObject[0, j].transform.position.y + 50, positionManager.buildingGameObject[0, j].transform.position.z), destroyMissile.transform.rotation);
                        StartCoroutine(DestroyTower(positionManager.buildingGameObject[0, j]));

                        positionManager.delete(positionManager.buildingGameObject[0, j].transform.position);
                    }
                    k++;
                }
            }
            GameManagerPartie.instance.enemyCoins = Pcoins;
            GameManagerPartie.instance.enemyCoinsTxt.text = GameManagerPartie.instance.enemyCoins.ToString();
            
        }
    }
    IEnumerator DestroyTower(GameObject go)
    {
        yield return new WaitForSeconds(2.2f);
        Destroy(go);

    }
    public void CarrotMissiles(int player)
    {
        if (player == 0)
        {


            GameObject go= MissilesInstantiated = Instantiate(Missiles);
            StartCoroutine(damagingAfterCarrotMissiles(MissilesInstantiated, player));
        }
        else
        {
            MissilesInstantiated = Instantiate(Missiles, Missiles.transform.position+new Vector3(0,0,-45), Missiles.transform.rotation);
            StartCoroutine(damagingAfterCarrotMissiles(MissilesInstantiated, player));

        }
        
    }
    IEnumerator damagingAfterCarrotMissiles(GameObject go,int i)
    {
        yield return new WaitForSeconds(2.5f);
        if (i == 0)
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
        }
        else
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {
                    positionManager.buildingGameObject[0, j].GetComponent<target>().takeDamage(75);
                }
            }
            GameManagerPartie.instance.player_.GetComponent<target>().takeDamage(50);
            GameManagerPartie.instance.playerTowerBase_.GetComponent<target>().takeDamage(75);
            Destroy(MissilesInstantiated);
        }
        
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
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[1, j] != null)
                {
                    positionManager.buildingGameObject[1, j].gameObject.GetComponent<target>().gainhealth(150);
                }

            }
            GameManagerPartie.instance.enemy_.GetComponent<target>().gainhealth(50);
            GameManagerPartie.instance.enemyTowerBase_.GetComponent<target>().gainhealth(200);
        }
    }
    public void carrotShield(int player)
    {
        if (player == 0)
        {
            GameObject CarrotWall_= Instantiate(CarrotWall);
            Instantiate(Carrot, new Vector3(-7.4f, -9, -37.2f), Carrot.transform.rotation, CarrotWall_.transform);
            Instantiate(Carrot, new Vector3(7.4f, -9, -37.2f), Carrot.transform.rotation, CarrotWall_.transform);
            Instantiate(Carrot, new Vector3(-4f, -9, -33.7f), Carrot.transform.rotation, CarrotWall_.transform);
            Instantiate(Carrot, new Vector3(0, -9, -33.7f), Carrot.transform.rotation, CarrotWall_.transform);
            Instantiate(Carrot, new Vector3(4, -9, -33.7f), Carrot.transform.rotation, CarrotWall_.transform);
            for(int i = 0; i < 5; i++)
            { int time = Random.Range(2, 6);
                LeanTween.moveY(CarrotWall_.transform.GetChild(i).gameObject, 7.3f, time).setEaseLinear();
            }
            StartCoroutine(HideCarrot(CarrotWall_));
            StartCoroutine( DestroyCarrotWall(CarrotWall_));


        }
        else
        {
            GameObject CarrotWallEnemy_ = Instantiate(CarrotWall, CarrotWall.transform.position + new Vector3(0, 0, 60), Quaternion.Euler(0, 180, 0));
            Instantiate(Carrot, new Vector3(-7.4f, -9, 39), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            Instantiate(Carrot, new Vector3(7.4f, -9, 39), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            Instantiate(Carrot, new Vector3(-4f, -9, 34), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            Instantiate(Carrot, new Vector3(0, -9, 34), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            Instantiate(Carrot, new Vector3(4, -9, 34), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            for (int i = 0; i < 5; i++)
            {
                int time = Random.Range(2, 6);
                LeanTween.moveY(CarrotWallEnemy_.transform.GetChild(i).gameObject, 7.3f, time).setEaseLinear();
            }
            StartCoroutine(HideCarrot(CarrotWallEnemy_));
            StartCoroutine( DestroyCarrotWall(CarrotWallEnemy_));



        }
    }
    IEnumerator HideCarrot(GameObject go)
    {
        yield return new WaitForSeconds(8);
        for(int i = 0; i < 5; i++)
        {
            LeanTween.moveY(go.transform.GetChild(i).gameObject, -9, 2).setEaseLinear();
        }
       
    }
    IEnumerator DestroyCarrotWall(GameObject go)
    {
        yield return new WaitForSeconds(10);
        Destroy(go);
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
        
       
    }
    public void shootMissileToEnemyBase(int i)
    {
        if (i == 0)
        {

            StartCoroutine(launchingDamageMissile());
        }
        
    }
    IEnumerator launchingDamageMissile()
    {
        for (int k = 0; k < 5; k++)
        {
            yield return new WaitForSeconds(0.5f);
            Vector3 vo = Calculatevelocity(GameManagerPartie.instance.enemyTowerPos, GameManagerPartie.instance.playerTowerPos + new Vector3(5, 5, 5), speed);
            GameObject go = Instantiate(DamageMissiles, GameManagerPartie.instance.playerTowerPos + new Vector3(k * 2, 5, 5), DamageMissiles.transform.rotation);
            go.GetComponent<Rigidbody>().velocity = vo;
            go.GetComponent<bullet>().changedam(150);
            go.GetComponent<bullet>().sender = gameObject;
        }
        
    }
    Vector3 Calculatevelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float sXZ = distanceXZ.magnitude;

        float Vxz = sXZ / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;

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
