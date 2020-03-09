using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIeasy : MonoBehaviour
{
    public static AIState CurrentState = AIState.start;
    public static TowerScript[] towers= new TowerScript[6];
    public byte[] buildPos = new byte[5];
    private TowerScript[] TowersWeCanBuild=new TowerScript[6];

    public TowerScript[] test = new TowerScript[6];
    public TowerScript[] test2 = new TowerScript[6];
    private float nextTimeToBuildRandom = 0;
    public float buildTimeRandom;
    private byte lastUnbuildedtower;
    public float speed = 5f;
    private byte strategy;
    private bool AiCanBuildRandomly = true;
    Vector3[] BuildPos = new[]{
        new Vector3(-15,2,15),
        new Vector3(-5,2,10),
        new Vector3(5,2,8),
        new Vector3(15,2,10),
        new Vector3(25,2,15),
        };
    public TowerScript[] Inventory1;
    public TowerScript[] Inventory2;
    public TowerScript[] Inventory3;
    public TowerScript[] Inventory4;
    public static byte minCostTower;
    //can replace with buildpos.x
    private float[] hiding = { -15, -5, 5, 15, 25 };
    private byte k = 0;
    public enum AIState
    {
        idle, die, shoot, build, hide,start,BuildRandom
    }
    public static void changeState(AIState state)
    {
        CurrentState = state;
        //Debug.Log(state);
    }

    private void Start()
    {
        strategy = (byte)Random.Range(0, 4);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log(CurrentState);
        switch(CurrentState)
        {
            case AIState.start:
                switch (strategy)
                {
                    case 0:
                        startStrategy1();
                        break;
                    case 1:
                        startStrategy2();
                        break;
                    case 2:
                        startStrategy3();
                        break;
                    case 3:
                        startStrategy4();
                        break;
                }

                changeState(AIState.idle);
                break;

            case AIState.idle:

                 // Debug.Log("hani fi idle state");
                if (!isMoving)
                {
                    float rand = Random.Range(1, 3);
                    StartCoroutine(move(rand));
                }
               
                if (!GameManagerPartie.instance.player_.activeSelf)
                {
                    changeState(AIState.shoot);
                }
                if (Time.time > nextTimeToBuildRandom)
                {
                    if ((positionManager.numBuildedTowers != lastUnbuildedtower)&& (towers[minCostTower].cost <= GameManagerPartie.instance.enemyCoins)&&AiCanBuildRandomly)
                    {
                        Debug.Log("ai can build = " + AiCanBuildRandomly);
                        lastUnbuildedtower = positionManager.numBuildedTowers;
                        changeState(AIState.BuildRandom);
                    }
                    nextTimeToBuildRandom = Time.time + buildTimeRandom;
                    lastUnbuildedtower = positionManager.numBuildedTowers;
                    AiCanBuildRandomly = true;
                }
                break;
            case AIState.BuildRandom:
                
                Debug.Log("buildrandom");
                buildPos = new byte[5];
                k = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (positionManager.buildingGameObject[1, i] == null)
                    {
                        buildPos[k] = (byte)i;
                        k++;
                    }
                }
                int randomBuildPos = Random.Range(0,k);
                k = 0;
                TowersWeCanBuild = new TowerScript[6];
                for (int i = 0; i < 6; i++)
                {
                    if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                    {
                        TowersWeCanBuild[k] = towers[i];
                        k++;
                    }
                }
                if (k > 0)
                {
                    int randomTower = Random.Range(0, k);
                    positionManager.add(towers[randomTower], BuildPos[buildPos[randomBuildPos]], GameManagerPartie.instance.enemylvl);
                }
                changeState(AIState.idle);
                break;
            case AIState.hide:
                Vector3 destination = new Vector3(0, transform.position.y, transform.position.z);
                float minDist = Mathf.Infinity;
                float currentPosX = transform.position.x;
                foreach (float x in hiding)
                {
                    float dist = Mathf.Abs(x - currentPosX);
                    if (dist < minDist)
                    {
                        destination.x = x;
                        minDist = dist;
                    }
                }
                
                transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
                if (Mathf.Abs(transform.position.x - destination.x) < 0.001f)
                {
                    changeState(AIState.idle);
                }
                break;

            case AIState.build:
                buildPos = new byte[5];
                k = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (positionManager.buildingGameObject[1, i] == null)
                    {
                        buildPos[k] = (byte)i;     
                        k++;                        
                    }
                }
                for (int i = 0; i < buildPos.Length; i++)
                {
                    StartCoroutine(building(i));
                }

                changeState(AIState.idle);
                break;

            case AIState.shoot:
                LeanTween.moveX(GameManagerPartie.instance.enemy_, 5, 0.2f).setEaseInOutSine();
                if (GameManagerPartie.instance.player_.activeSelf)
                {
                    changeState(AIState.idle);
                }

                break;
            case AIState.die:
                //player win
                break;
        }
    }
    bool isMoving = false;
    IEnumerator move(float time)
    {
        isMoving = true;
        float rand = Random.Range(-18, 28);
        LeanTween.moveX(GameManagerPartie.instance.enemy_, rand, time).setEaseLinear();
        yield return new WaitForSeconds (time);
        isMoving = false;
    }
    IEnumerator building (int i)
    {
        AiCanBuildRandomly = false;
        yield return new WaitForSeconds(3);
        switch (strategy)
        {
            case 0:
                strategy1(buildPos[i]);
                break;
            case 1:
                strategy2(buildPos[i]);
                break;
            case 2:
                strategy3(buildPos[i]);
                break;
            case 3:
                strategy4(buildPos[i]);
                break;
        }   
    }

    public void startStrategy1()
    {
        minCostTower = 5;
        towers = Inventory1;
        if (positionManager.buildingGameObject[1, 2] == null)
        {
            int random = Random.Range(0, 2);
            if (random == 0 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 1 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if(positionManager.buildingGameObject[1,0]==null)
        {
            int random = Random.Range(2, 4);
            
            if (random == 2 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else
                return;   
        }
        if (positionManager.buildingGameObject[1, 4] == null)
        {
            int random = Random.Range(2, 4);
            
            if (random == 2 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if (positionManager.buildingGameObject[1, 1] == null)
        {
            
            int random = Random.Range(4, 6);
          
            if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 5 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if (positionManager.buildingGameObject[1, 3] == null)
        {
            int random = Random.Range(4, 6);

            if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 5 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        positionManager.aiCanBuild();
    }
    public void strategy1 (byte h)
    {
        int j;
        switch (BuildPos[h].x)
        {
            case -15:
                if (positionManager.buildingTowerScript[0, 0] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 0].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);
                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                                changeState(AIState.idle);
                            break;

                        case "mirror tower":
                             j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins)&&((towers[i].name== "block tower")||(towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                             j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                           

                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ( (towers[i].name == "healing tower") || (towers[i].name == "lazer tower") ))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") ))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;         
                    }
                }

                break;
            case -5:
                if (positionManager.buildingTowerScript[0, 1] != null)
                {
                    Debug.Log("positionManager.buildingTowerScript[0, 1] != null");

                    switch (positionManager.buildingTowerScript[0, 1].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                    }
                }
                break;
            case 5:
                if (positionManager.buildingTowerScript[0, 2] != null)
                {

                    switch (positionManager.buildingTowerScript[0, 2].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                    }
                }
                break;
            case 15:
                if (positionManager.buildingTowerScript[0, 3] != null)
                {
                   
                    switch (positionManager.buildingTowerScript[0, 3].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                    }
                    
                }

                break;
            case 25:
                if (positionManager.buildingGameObject[0, 4] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 4].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                    }
                }
                break;


        }
       
    }
    public void startStrategy2()
    {
       
        minCostTower = 3;
        towers = Inventory2;
        if (positionManager.buildingGameObject[1, 2] == null)
        {
            if (towers[2].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[2], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if(positionManager.buildingGameObject[1,3]==null)
        {

            if (towers[3].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[3], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else
                return;   
        }
        if (positionManager.buildingGameObject[1, 1] == null)
        {
            if (towers[4].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[4], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        
        //changeState(AIState.BuildRandom);
        
        positionManager.aiCanBuild();

    }
    public void strategy2(byte h)
    {
        
        int j;
        switch (BuildPos[h].x)
        {
            case -15:
                if (positionManager.buildingTowerScript[0, 0] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 0].name)
                    {
                        case "block tower":

                            if (towers[2].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[2], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            
                                if (towers[1].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    positionManager.add(towers[1], BuildPos[h], GameManagerPartie.instance.enemylvl);
                                }     
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case -5:
                if (positionManager.buildingTowerScript[0, 1] != null)
                {
                    Debug.Log("positionManager.buildingTowerScript[0, 1] != null");

                    switch (positionManager.buildingTowerScript[0, 1].name)
                    {
                        case "block tower":

                            if (towers[2].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[2], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            if (towers[1].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[1], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 5:
                if (positionManager.buildingTowerScript[0, 2] != null)
                {

                    switch (positionManager.buildingTowerScript[0, 2].name)
                    {
                        case "block tower":

                            if (towers[2].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[2], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            if (towers[1].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[1], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 15:
                if (positionManager.buildingTowerScript[0, 3] != null)
                {

                    switch (positionManager.buildingTowerScript[0, 3].name)
                    {
                        case "block tower":

                            if (towers[2].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[2], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            if (towers[1].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[1], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 25:
                if (positionManager.buildingGameObject[0, 4] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 4].name)
                    {
                        case "block tower":

                            if (towers[2].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[2], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            if (towers[1].cost <= GameManagerPartie.instance.enemyCoins)
                            {
                                positionManager.add(towers[1], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "lazer tower") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
                


        }
        AiCanBuildRandomly = true;

    }
    public void startStrategy3()
    {
        minCostTower = 2;
        towers = Inventory3;
        if (positionManager.buildingGameObject[1, 2] == null)
        {
            int random = Random.Range(3, 5);
            if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if(positionManager.buildingGameObject[1,0]==null)
        {
            int random = Random.Range(0, 2);
            
            if (random == 0 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 1 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else
                return;   
        }
        if (positionManager.buildingGameObject[1, 4] == null)
        {
            int random = Random.Range(0, 2);
            
            if (random == 0 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 1 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        

        changeState(AIState.BuildRandom);
        positionManager.aiCanBuild();

    }
    public void strategy3(byte h)
    {

        int j;
        switch (BuildPos[h].x)
        {
            case -15:
                if (positionManager.buildingTowerScript[0, 0] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 0].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon")  || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "block tower") ))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mortar") || (towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") ||  (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon")  || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower") ))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case -5:
                if (positionManager.buildingTowerScript[0, 1] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 1].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mortar") || (towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 5:
                if (positionManager.buildingTowerScript[0, 2] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 2].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mortar") || (towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 15:
                if (positionManager.buildingTowerScript[0, 3] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 3].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mortar") || (towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 25:
                if (positionManager.buildingTowerScript[0, 4] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 4].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mortar") || (towers[i].name == "x-bow") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "freezing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "x-bow") || (towers[i].name == "mortar") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;



        }
        AiCanBuildRandomly = true;

    }
    public void startStrategy4()
    {
        minCostTower = 0;
        towers = Inventory4;
        if (positionManager.buildingGameObject[1, 2] == null)
        {
            int random = Random.Range(0, 2);
            if (random == 0 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 1 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if (positionManager.buildingGameObject[1, 0] == null)
        {
            int random = Random.Range(4, 6);

            if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 5 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if (positionManager.buildingGameObject[1, 4] == null)
        {
            int random = Random.Range(4, 6);

            if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 5 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if (positionManager.buildingGameObject[1, 1] == null)
        {
            int random = Random.Range(2, 4);

            if (random == 2 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if (positionManager.buildingGameObject[1, 3] == null)
        {
            int random = Random.Range(2, 4);

            if (random == 2 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }

        changeState(AIState.BuildRandom);
        positionManager.aiCanBuild();

    }
    public void strategy4(byte h)
    {

        int j;
        switch (BuildPos[h].x)
        {
            case -15:
                if (positionManager.buildingTowerScript[0, 0] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 0].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) &&  (towers[i].name == "freezing tower"))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "tesla")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower") ))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower") ))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case -5:
                if (positionManager.buildingTowerScript[0, 1] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 1].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && (towers[i].name == "freezing tower"))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "tesla")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 5:
                if (positionManager.buildingTowerScript[0, 2] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 2].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && (towers[i].name == "freezing tower"))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "tesla")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 15:
                if (positionManager.buildingTowerScript[0, 3] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 3].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && (towers[i].name == "freezing tower"))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "tesla")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;
            case 25:
                if (positionManager.buildingTowerScript[0, 4] != null)
                {
                    switch (positionManager.buildingTowerScript[0, 4].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && (towers[i].name == "freezing tower"))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":

                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "lazer tower") || (towers[i].name == "tesla")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "tesla") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "canon") || (towers[i].name == "block tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "freezing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(TowersWeCanBuild[(byte)random], BuildPos[h], GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                    }
                }
                break;



        }
        AiCanBuildRandomly = true;

    }
}