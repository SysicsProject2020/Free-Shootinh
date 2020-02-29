using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFunctions : MonoBehaviour
{
    public GameObject shield;
    public static MagicFunctions instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void destroyAll(int player)
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                if (positionManager.buildingGameObject[i,j] != null)
                {
                    Destroy(positionManager.buildingGameObject[i, j].gameObject);
                    positionManager.buildingGameObject[i, j] = null;
                    positionManager.buildingTowerScript[i, j] = null;
                }
            }
        }
    }
    public void Shield(int player)
    {
        if (player == 0)
        {
            Instantiate(shield, GameManagerPartie.instance.playerTowerPos, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Instantiate(shield, GameManagerPartie.instance.enemyTowerPos, Quaternion.Euler(0, 0, 0));
        }
    }
    public void destroyEnemyTower(int player)
    {
        if (player == 0)
        {
            
        }
        else
        {
            
        }
    }
    public void Accelerate(int player)
    {
        if (player == 0)
        {
           
        }
        else
        {
           
        }
    }
    public void Clone(int player)
    {
        if (player == 0)
        {

        }
        else
        {

        }
    }
    public void FreezTower(int player)
    {
        if (player == 0)
        {

        }
        else
        {

        }
    }
    public void SeeCards(int player)
    {
        if (player == 0)
        {

        }
        else
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
