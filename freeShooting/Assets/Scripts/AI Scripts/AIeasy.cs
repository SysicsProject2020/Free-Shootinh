using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIeasy : MonoBehaviour
{
    public static AIState CurrentState;
    public static TowerScript[] towers= new TowerScript[6];
    byte[] buildPos = new byte[5];
    TowerScript[] TowersWeCanBuild=new TowerScript[6];

    //public TowerScript[] test = new TowerScript[6];
   //public TowerScript[] test2 = new TowerScript[6];
    private float nextTimeToBuildRandom;
    //public float buildTimeRandom;
    //private byte lastUnbuildedtower;
    public float speed = 5f;
    private byte strategy;
    private bool AiCanBuildRandomly = true;
    
    //Animator anim;
    Vector3[] BuildPos = new[]{
        new Vector3(-16.52f,2,18.78f),
        new Vector3(-8.27f,2,13.36f),
        new Vector3(0,2,7.94f),
        new Vector3(8.27f,2,13.36f),
        new Vector3(16.52f,2,18.78f),
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
        idle, die, shoot, build, hide,start,BuildRandom,Magic1,Magic2,wait
    }
    public static void changeState(AIState state)
    {
        CurrentState = state;
        //Debug.Log(state);
    }

    private void Start()
    {
        strategy = 0;
        nextTimeToBuildRandom = 0;
        CurrentState = AIState.start;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch(CurrentState)
        {
            case AIState.start:
                switch (strategy)
                {
                    case 0:
                        StartCoroutine(startStrategy1());
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
                changeState(AIState.wait);
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
                    if ((positionManager.numBuildedTowers < 5) && (towers[minCostTower].cost <= GameManagerPartie.instance.enemyCoins) && AiCanBuildRandomly)
                    {
                        Debug.Log("send to build random .mincost = " + towers[minCostTower].cost + "enemy coin= " + GameManagerPartie.instance.enemyCoins);
                        //lastUnbuildedtower = positionManager.numBuildedTowers;
                        changeState(AIState.BuildRandom);
                    }
                    nextTimeToBuildRandom = Time.time + Random.Range(2,6);
                    //lastUnbuildedtower = positionManager.numBuildedTowers;
                    //AiCanBuildRandomly = true;
                }
                break;
            case AIState.Magic1:
                switch (GameManager.instance.getPlayer().name)
                {
                    case "Panda":
                        MagicFunctions.instance.PandaMagic1(1);
                            break;
                    case "Pig":
                        MagicFunctions.instance.PigMagic1(1);
                        break;
                    case "Rabbit":
                        MagicFunctions.instance.RabbitMagic1(1);
                        break;
                    case "Taurus":
                        MagicFunctions.instance.TaurusMagic1(1);
                        break;

                }
                changeState(AIState.idle);
                break;
            case AIState.Magic2:
                switch (GameManager.instance.getPlayer().name)
                {
                    case "Panda":
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
                        int randomPos = Random.Range(0, k);
                        int randomTower = Random.Range(0, 6);
                        GameManagerPartie.instance.enemyCoins += towers[randomTower].cost;
                        positionManager.add(towers[randomTower], BuildPos[buildPos[randomPos]], GameManagerPartie.instance.enemylvl);
                        break;
                    case "Pig":
                        MagicFunctions.instance.PigMagic2();
                        break;
                    case "Rabbit":
                        MagicFunctions.instance.RabbitMagic2(1);
                        break;
                    case "Taurus":
                        MagicFunctions.instance.TaurusMagic2(1);
                        break;

                }
                changeState(AIState.idle);
                break;

            case AIState.BuildRandom:
                randombuild();
                changeState(AIState.wait);
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
                GameManagerPartie.instance.enemy_.transform.GetChild(0).GetComponent<Animator>().SetFloat("x", 0.5f);
                transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
                if (Mathf.Abs(transform.position.x - destination.x) < 0.005f)
                {
                    changeState(AIState.idle);
                }
                break;

            case AIState.build:
                StartCoroutine(building());
                changeState(AIState.wait);
                break;

            case AIState.shoot:
                LeanTween.moveX(GameManagerPartie.instance.enemy_, 0, 0.2f).setEaseInOutSine();
                GameManagerPartie.instance.enemy_.transform.GetChild(0).GetComponent<Animator>().SetFloat("x", 0.5f);
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
        float rand = Random.Range(-19, 19);
        if (rand < GameManagerPartie.instance.enemy_.transform.position.x)
        {
            GameManagerPartie.instance.enemy_.transform.GetChild(0).GetComponent<Animator>().SetFloat("x", 1);
        }
        else
        {
            GameManagerPartie.instance.enemy_.transform.GetChild(0).GetComponent<Animator>().SetFloat("x", 0);
        }
        LeanTween.moveX(GameManagerPartie.instance.enemy_, rand, time).setEaseLinear();
        yield return new WaitForSeconds (time);
        isMoving = false;
    }
    void randombuild()
    {
        Debug.Log("start build random");
        GameManagerPartie.instance.enemy_.transform.GetChild(0).GetComponent<Animator>().SetFloat("x", 0.5f);
        k = 0;
        TowersWeCanBuild = new TowerScript[6];
        for (int i = 0; i < 6; i++)
        {
            if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
            {
                Debug.Log(towers[i].cost + " <= " + GameManagerPartie.instance.enemyCoins);
                TowersWeCanBuild[k] = towers[i];
                k++;
            }
        }
        if (k > 0)
        {
            int randomTower = Random.Range(0, k);

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
            int randomBuildPos = Random.Range(0, k);

            if (k > 0)
            {
                //yield return new WaitForSeconds(time);

                positionManager.add(TowersWeCanBuild[randomTower], BuildPos[buildPos[randomBuildPos]], GameManagerPartie.instance.enemylvl);

            }
        }
        changeState(AIState.idle);
        Debug.Log("end build random and back to idle");
    }
    public static byte toBuilTower;
    IEnumerator building()
    {
        AiCanBuildRandomly = false;
        yield return new WaitForSeconds(Random.Range(1,2));
        Debug.Log("start building coroutine");
        GameManagerPartie.instance.enemy_.transform.GetChild(0).GetComponent<Animator>().SetFloat("x", 0.5f);       
        switch (strategy)
        {
            case 0:
                strategy1(toBuilTower);
                break;
            case 1:
                strategy2(toBuilTower);
                break;
            case 2:
                strategy3(toBuilTower);
                break;
            case 3:
                strategy4(toBuilTower);
                break;
        }
        /*
        buildPos = new byte[5];
        k = 0;
        for (short i = 0; i < 5; i++)
        {
            if (positionManager.buildingGameObject[1, i] == null)
            {
                buildPos[k] = (byte)i;
                k++;
            }
        }
        if (k>0)
        {
            for (int i = 0; i < buildPos.Length; i++)
            {
                if (positionManager.buildingGameObject[1, buildPos[i]] == null)
                {
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
                    yield return new WaitForSeconds(1);
                }
            }
        }
        */
        AiCanBuildRandomly = true;
        changeState(AIState.idle);
        Debug.Log("end building coroutine and back to idle");
    }

    IEnumerator startStrategy1()
    {
        minCostTower = 5;
        towers = Inventory1;
        if (positionManager.buildingGameObject[1, 2] == null)
        {
            int random = Random.Range(0, 2);
            if (random == 0 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 1 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else
            {
                CurrentState = AIState.idle;
                Debug.Log("end strategie 1");
                yield break;
            }

        }
        if (positionManager.buildingGameObject[1,0]==null)
        {
            int random = Random.Range(2, 4);
            
            if (random == 2 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else
            {
                CurrentState = AIState.idle;
                Debug.Log("end strategie 1");
                yield break;
            }

        }
        if (positionManager.buildingGameObject[1, 4] == null)
        {
            int random = Random.Range(2, 4);
            
            if (random == 2 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else
            {
                CurrentState = AIState.idle;
                Debug.Log("end strategie 1");
                yield break;
            }
        }
        if (positionManager.buildingGameObject[1, 1] == null)
        {
            
            int random = Random.Range(4, 6);
          
            if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 5 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else
            {
                CurrentState = AIState.idle;
                Debug.Log("end strategie 1");
                yield break;
            }
        }
        if (positionManager.buildingGameObject[1, 3] == null)
        {
            int random = Random.Range(4, 6);

            if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 5 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                yield return new WaitForSeconds(1);
                positionManager.add(towers[(byte)random], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else
            {
                CurrentState = AIState.idle;
                Debug.Log("end strategie 1");
                yield break;
            }
        }
        CurrentState = AIState.idle;
        Debug.Log("end strategie 1");
    }
    public void strategy1 (byte h)
    {
        int j;
        switch (h)
        {
            case 0:
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
            case 1:
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
            case 2:
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
            case 3:
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
            case 4:
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
        Debug.Log("end strategie 1");
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
        
       // positionManager.aiCanBuild();

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
        //positionManager.aiCanBuild();

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
        //positionManager.aiCanBuild();

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