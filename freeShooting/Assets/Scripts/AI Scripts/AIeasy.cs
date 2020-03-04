using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIeasy : MonoBehaviour
{
    public static AIState CurrentState = AIState.idle;
    public  TowerScript[] towers= new TowerScript[6];
    private Vector3 buildPos;
    public TowerScript[] TowersWeCanBuild=new TowerScript[6];

    private GameObject player;
    private short currentHealth;
    public float speed = 5f;
    Vector3[] BuildPos = new[]{
        new Vector3(-15,2,15),
        new Vector3(-5,2,10),
        new Vector3(5,2,8),
        new Vector3(15,2,10),
        new Vector3(25,2,15),
        };

    //can replace with buildpos.x
    private float[] hiding = { -15, -5, 5, 15, 25 };


    public enum AIState
    {
        idle, die, shoot, build, hide
    }
    public static void changeState(AIState state)
    {
        CurrentState = state;
        //Debug.Log(state);
    }

    void selectPlayer() { player = GameManagerPartie.instance.player_; }
    public byte minCostTower = 0;
    // Start is called before the first frame update
    void Start()
    {
        //towers = GameManagerPartie.instance.EnemySelectedTowers;
        selectPlayer();
        //enemyBuildZone = GameObject.FindGameObjectsWithTag("EnemyTowersZones");
        //currentHealth = player.Get_health();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(CurrentState);

        switch(CurrentState)
        {
            case AIState.idle:
                //  Debug.Log("hani fi idle state");

                if (!isMoving)
                {
                    float rand = Random.Range(1, 3);
                    StartCoroutine(move(rand));


                }
                for (int i = 0; i < 5; i++)
                {
                    if (positionManager.buildingGameObject[1, i] == null && towers[minCostTower].cost <= GameManagerPartie.instance.enemyCoins)
                    {
                        buildPos = BuildPos[i];
                        changeState(AIState.build);
                        //Debug.Log("fama 7aja far4a fi position manager");
                        //return;
                    }
                }

                //Debug.Log(isMoving);
               
               
              /*  if((positionManager.buildingGameObject[0, 3] == null))
                {
                    LeanTween.moveX(GameManagerPartie.instance.enemy_, 5, 0.2f).setEaseInOutSine();
                }*/
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
                int j = 0;
                //Debug.Log("hani fi build state");
                for (int i = 0; i < 5; i++)
                {
                   
                    if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                    {
                        
                        //Debug.Log(i);
                        TowersWeCanBuild[j] = towers[i];
                        j++;
                       
                        //towersWeCanBuild[j] = GameManagerPartie.instance.EnemySelectedTowers[i];
                        //towersWeCanBuild[j] = GameManager.instance.GetSelectedTowers()[i];
                    }
                   
                }
                positionManager.add(TowersWeCanBuild[Random.Range(0, j - 1)], buildPos, GameManagerPartie.instance.enemylvl);
                changeState(AIState.idle);
                break;

            case AIState.shoot:
                /*destination = choosingDestination();
                transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
                if (transform.position == destination)
                {
                    changeState(AIState.idle);
                }*/
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
        Debug.Log("moved");
    }

    Vector3 choosingDestination()
    {
        Vector3 position;
        do
        {
            position = new Vector3(Random.Range(-17.8f, 28.8f), 1.8f, 25);
        }
        while (position.x < 5);
        return position;
    } 
    Vector3 choosingPos() { return BuildPos[Random.Range(0, 5)];}

    TowerScript choosingBuild() { return towers[Random.Range(0, 5)]; }
    void seePlayer() { //see player build on idle state 
    }
    

}