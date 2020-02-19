using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWithSliderBar : MonoBehaviour
{
     public void MovePlayer(float position)
    {
        GameManagerPartie.player_.transform.position = new Vector3(position, GameManagerPartie.playerPos.y, GameManagerPartie.playerPos.z);
        Debug.Log(GameManagerPartie.player_.transform.position);
    }
}
