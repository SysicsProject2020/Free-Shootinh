using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIeasy : MonoBehaviour
{
    public static AIState CurrentState = AIState.build;
    //private GameObject[] enemyBuildZone = new GameObject[5];
    public static TowerScript[] towers= new TowerScript[6];
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
    bool[] builded = new[]
    {
        false,false,false,false,false,
    };
    //can replace with buildpos.x
    private float[] hiding = { -15, -5, 5, 15, 25 };


    public enum AIState
    {
        idle, die, shoot,build,hide
    }
    public static void changeState(AIState state)
    {
        CurrentState = state;
        Debug.Log(state);
    }

    void selectPlayer() { player = GameManagerPartie.player_; }

    // Start is called before the first frame update
    void Start()
    {
        towers = GameManagerPartie.EnemySelectedTowers;
        selectPlayer();
        //enemyBuildZone = GameObject.FindGameObjectsWithTag("EnemyTowersZones");
        //currentHealth = player.Get_health();
    }

    // Update is called once per frame
    void Update()
    {

        switch(CurrentState)
        {
            case AIState.idle:
                

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

                /*Vector3 randombuild = choosingPos();
                randombuild.y += 2f;
                TowerScript tower =  choosingBuild();

                GameObject go = Instantiate(tower.prefab, randombuild, Quaternion.Euler(0,0,0));
                changeState(AIState.idle);*/

                for (int i = 0; i < 5; i++)
                {
                    GameObject go = Instantiate(towers[i].prefab, BuildPos[i], Quaternion.Euler(0, 0, 0));
                    Debug.Log("ffds");
                }
                changeState(AIState.idle);
                break;



            case AIState.shoot:
                /*destination = choosingDestination();
                transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
                if (transform.position == destination)
                {
                    changeState(AIState.idle);
                }*/
                break;



            case AIState.die:
                //player win
                break;
        }


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