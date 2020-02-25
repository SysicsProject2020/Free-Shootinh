using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWithSliderBar : MonoBehaviour
{
    public float speed =10f;
    
    GameObject enemy;
    GameObject enemytowerBase;

   
    private void Start()
    {
        enemy = GameManagerPartie.enemy_;
        enemytowerBase = GameManagerPartie.instance.enemyTowerBase_;
    }

    private void Update()
    {
       // rotate();
    }
    public void rotate()
    {
        if (enemy.activeSelf)
        {
            Vector3 relativePos = enemy.transform.position - transform.position;
            Quaternion rotObject = Quaternion.LookRotation(relativePos, Vector3.up);
            rotObject = Quaternion.Euler(transform.rotation.x, rotObject.eulerAngles.y, transform.rotation.z);
            transform.rotation = rotObject;
        }
        else
        {
            Vector3 relativePos = enemytowerBase.transform.position - transform.position;
            Quaternion rotObject = Quaternion.LookRotation(relativePos, Vector3.up);
            rotObject = Quaternion.Euler(transform.rotation.x, rotObject.eulerAngles.y, transform.rotation.z);
            transform.rotation = rotObject;
        }
    }
    public void MovePlayer(float position)
    {
        Vector3 pos = new Vector3(position, GameManagerPartie.playerPos.y, GameManagerPartie.playerPos.z);
        //GameManagerPartie.player_.transform.position = Vector3.Lerp(GameManagerPartie.player_.transform.position, pos, speed * Time.deltaTime);
        GameManagerPartie.player_.transform.position = new Vector3(position, GameManagerPartie.playerPos.y, GameManagerPartie.playerPos.z);
       // Debug.Log(GameManagerPartie.player_.transform.position);
    }
}
