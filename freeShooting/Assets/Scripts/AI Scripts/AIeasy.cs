using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIeasy : MonoBehaviour
{
    public static AIState CurrentState = AIState.idle;
    private GameObject[] enemyBuildZone = new GameObject[5];
    public static TowerScript[] selectedcards= new TowerScript[6];
    private PlayerScript player;
    private short currentHealth;
    public float speed = 5f;



    public enum AIState
    {
        idle, die, move,build,hide
    }
    public static void changeState(AIState state)
    {
        CurrentState = state;
    }
    // Start is called before the first frame update
    void Start()
    {
        selectedcards= choosingCards();
        selectPlayer();
        enemyBuildZone = GameObject.FindGameObjectsWithTag("EnemyTowersZones");
        currentHealth = player.Get_health();
        
    }

    // Update is called once per frame
    void Update()
    {

        switch(CurrentState)
        {
            case AIState.idle:
                

                break;
            case AIState.hide:


                break;

            case AIState.build:
               /* PlayerScript tower =  choosingBuild();
                Transform zone = choosingBuildZone();
                Vector3 towerpos = new Vector3(zone.transform.position.x, transform.position.y, zone.transform.position.z);
                GameObject go = Instantiate(tower.prefab, towerpos, Quaternion.Euler(0, 0, 0));
                changeState(AIState.idle);*/
                break;



            case AIState.move:
                Vector3 destination = choosingDestination();
                transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
                if (transform.position == destination)
                {
                    changeState(AIState.idle);
                }
                break;



            case AIState.die:

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
    PlayerScript choosingBuild() { return null; }
    Transform choosingBuildZone() { return null; }
    TowerScript[] choosingCards() { return GameManager.instance.GetSelectedTowers(); }
    void selectPlayer() { player = GameManager.instance.getPlayer(); }
}