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
    private Coroutine testFree;
    private short currentCoins;
    [Header("Magic:RabitMagic2")]
    public GameObject Missiles;
    
    
    
    [Header("Magic:Destroy Tower")]
    public GameObject destroyMissile;
    [Header("Magic:Destroy All Towers")]
    public GameObject Explosion;
    private GameObject ExplosionInstatiated;
    [Header("Magic:DirectShot")]
    
    public GameObject DamageMissiles;
    public float speed;
    [Header("Magic:RabbitMagic1")]
    public GameObject Carrot;
    public GameObject CarrotWall;



    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    
    public void PigMagic2()
    {
        ExplosionInstatiated= Instantiate(Explosion, new Vector3(0, 12, 0), Explosion.transform.rotation);
        StartCoroutine(destoryAllTowers(ExplosionInstatiated));
    }
    IEnumerator destoryAllTowers(GameObject go)
    {
        for(int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(0.3f);
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
        GameManagerPartie.instance.ChangeInteractableSpritesPrice();
        Destroy(go);


    }
    public void TaurusMagic1(int player)
    {
        if (player == 0)
        {
            GameObject ShieldInstantiated=Instantiate(shield, new Vector3(GameManagerPartie.instance.playerTowerPos.x, GameManagerPartie.instance.playerTowerPos.y+12, GameManagerPartie.instance.playerTowerPos.z) , Quaternion.Euler(0, 0, 0));
            StartCoroutine(removeShield(ShieldInstantiated));
        }
        else
        {
            GameObject ShieldInstantiated = Instantiate(shield, new Vector3(GameManagerPartie.instance.enemyTowerPos.x,GameManagerPartie.instance.enemyTowerPos.y+12, GameManagerPartie.instance.enemyTowerPos.z), Quaternion.Euler(0, 0, 0));
            StartCoroutine(removeShield(ShieldInstantiated));
        }
    }
    IEnumerator removeShield(GameObject go)
    {
        yield return new WaitForSeconds(10);
        Destroy(go);
    }
   
    public void PigMagic1(int player)
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
            GameManagerPartie.instance.ChangeInteractableSpritesPrice();

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
    public void RabbitMagic1(int player)
    {
        if (player == 0)
        {


            GameObject  MissilesInstantiated = Instantiate(Missiles);
            StartCoroutine(damagingAfterCarrotMissiles(MissilesInstantiated, player));
        }
        else
        {
            GameObject MissilesInstantiated = Instantiate(Missiles, Missiles.transform.position+new Vector3(0,0,-45), Missiles.transform.rotation);
            StartCoroutine(damagingAfterCarrotMissiles(MissilesInstantiated, player));

        }
        
    }
    IEnumerator damagingAfterCarrotMissiles(GameObject go,int i)
    {
        yield return new WaitForSeconds(3.2f);
        if (i == 0)
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[1, j] != null)
                {
                    GameManagerPartie.instance.playerDamage -= 75;
                    positionManager.buildingGameObject[1, j].GetComponent<target>().takeDamage(75);
                }
            }
            GameManagerPartie.instance.playerDamage -= 125;
            GameManagerPartie.instance.enemy_.GetComponent<target>().takeDamage(50);
            GameManagerPartie.instance.enemyTowerBase_.GetComponent<target>().takeDamage(75);
            Destroy(go);
        }
        else
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {
                    GameManagerPartie.instance.enemyDamage -= 75;
                    positionManager.buildingGameObject[0, j].GetComponent<target>().takeDamage(75);
                }
            }
            GameManagerPartie.instance.enemyDamage -= 125;
            GameManagerPartie.instance.player_.GetComponent<target>().takeDamage(50);
            GameManagerPartie.instance.playerTowerBase_.GetComponent<target>().takeDamage(75);
            Destroy(go);
        }
        
    }
   
    public void PandaMagic2(int player)
    {
        if (player == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                GameManagerPartie.instance.itemParent.transform.GetChild(i).GetComponent<Button>().interactable = true;
                // GameManagerPartie.instance.itemParent.transform.GetChild(i).get
                GameManagerPartie.instance.itemParent.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = GameManagerPartie.instance.towersSelected[i].image;
                GameManagerPartie.instance.itemParent.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                GameManagerPartie.instance.itemParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "0";
            }

            testFree = StartCoroutine(testfreeCoroutine());
            currentCoins = GameManagerPartie.instance.playerCoins;
        }

    }
    void returnCardsValues(int player)
    {
        //GameManagerPartie.instance.ChangeInteractableSpritesPrice();
        GameManagerPartie.instance.ChangeCosts();
        StopCoroutine(testFree);
    }
    // Update is called once per frame
    public void PandaMagic1(int player)
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
            if (GameManagerPartie.instance.enemyTowerBase_ != null)
            {
                GameManagerPartie.instance.enemyTowerBase_.GetComponent<target>().gainhealth(200);
            }
            
        }
    }
    public void RabbitMagic2(int player)
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
    IEnumerator testfreeCoroutine()
    {
        while (true)
        {
            if (currentCoins > GameManagerPartie.instance.playerCoins)
            {
                GameManagerPartie.instance.playerCoins = currentCoins;
                GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
                returnCardsValues(0);
            }
            else
                currentCoins = GameManagerPartie.instance.playerCoins;
            yield return null;
        }    
    }
    public void TaurusMagic2(int i)
    {



            StartCoroutine(launchingDamageMissile(i));
        
        
    }
    IEnumerator launchingDamageMissile(int player)
    {

        if (player == 0)
        {
            for (int k = 0; k < 5; k++)
            {
                yield return new WaitForSeconds(0.5f);
                Vector3 vo = Calculatevelocity(GameManagerPartie.instance.enemyTowerPos, GameManagerPartie.instance.playerTowerPos + new Vector3(k, 20, 5), speed);
                GameObject go = Instantiate(DamageMissiles, GameManagerPartie.instance.playerTowerPos + new Vector3(k, 30, 5), DamageMissiles.transform.rotation);
                go.GetComponent<Rigidbody>().velocity = vo;
                go.GetComponent<bullet>().changedam(100);
                go.GetComponent<bullet>().sender = gameObject;
                GameManagerPartie.instance.playerDamage -= 100;
            }
        }
        else
        {
            for (int k = 0; k < 5; k++)
            {
                yield return new WaitForSeconds(0.5f);
                Vector3 vo = Calculatevelocity(GameManagerPartie.instance.playerTowerPos,GameManagerPartie.instance.enemyTowerPos + new Vector3(k, 20, 25), speed);
                GameObject go = Instantiate(DamageMissiles, GameManagerPartie.instance.enemyTowerPos + new Vector3(k, 30, 5), DamageMissiles.transform.rotation);
                go.GetComponent<Rigidbody>().velocity = vo;
                go.GetComponent<bullet>().changedam(100);
                go.GetComponent<bullet>().sender = gameObject;
                GameManagerPartie.instance.enemyDamage -= 100;
            }
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
