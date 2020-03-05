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

    public float speed = 5f;
    Vector3[] BuildPos = new[]{
        new Vector3(-15,2,15),
        new Vector3(-5,2,10),
        new Vector3(5,2,8),
        new Vector3(15,2,10),
        new Vector3(25,2,15),
        };
    public TowerScript[] Inventory1;
    public static byte minCostTower;
    //can replace with buildpos.x
    private float[] hiding = { -15, -5, 5, 15, 25 };

    public enum AIState
    {
        idle, die, shoot, build, hide,start
    }
    public static void changeState(AIState state)
    {
        CurrentState = state;
        //Debug.Log(state);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < 5; i++)
        {
            test[i] = positionManager.buildingTowerScript[0, i];
            test[i] = positionManager.buildingTowerScript[1, i];
        }
        
        //Debug.Log(CurrentState);
        switch(CurrentState)
        {
            case AIState.start:
                startStrategy1();
                changeState(AIState.idle);
                break;

            case AIState.idle:

                //  Debug.Log("hani fi idle state");
                if (!isMoving)
                {
                    float rand = Random.Range(1, 3);
                    StartCoroutine(move(rand));
                }
               
                if (!GameManagerPartie.instance.player_.activeSelf)
                {
                    changeState(AIState.shoot);
                }        
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
                int k = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (positionManager.buildingGameObject[1, i] == null)
                    {
                        buildPos[k] = (byte)i;
                        k++;
                        
                        //break;
                    }
                }
                for (int i = 0; i < k; i++)
                {
                    strategy1(buildPos[i]);
                }

                changeState(AIState.idle);
                break;

            case AIState.shoot:
                LeanTween.moveX(GameManagerPartie.instance.enemy_, 5, 0.2f).setEaseInOutSine();
                if (GameManagerPartie.instance.player_.activeSelf)
                {
                    changeState(AIState.idle);
                }
                /*
                if ((positionManager.testbuild > 0) && (towers[minCostTower].cost <= GameManagerPartie.instance.enemyCoins))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (positionManager.buildingGameObject[1, i] == null)
                        {
                            buildPos = BuildPos[i];
                            changeState(AIState.build);
                        }
                    }
                }*/
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
        //Debug.Log("moved");
    }

    public void startStrategy1()
    {
        minCostTower = 4;
        towers = Inventory1;
        /*if (positionManager.buildingGameObject[1, 2] == null)
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
        }*/
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


}