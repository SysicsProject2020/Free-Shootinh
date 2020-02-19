using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWithSliderBar : MonoBehaviour
{
    public float speed =10f;
     public void MovePlayer(float position)
    {
        Vector3 pos = new Vector3(position, GameManagerPartie.playerPos.y, GameManagerPartie.playerPos.z);
        //GameManagerPartie.player_.transform.position = Vector3.Lerp(GameManagerPartie.player_.transform.position, pos, speed * Time.deltaTime);
        GameManagerPartie.player_.transform.position = new Vector3(position, GameManagerPartie.playerPos.y, GameManagerPartie.playerPos.z);
        Debug.Log(GameManagerPartie.player_.transform.position);
    }
}
