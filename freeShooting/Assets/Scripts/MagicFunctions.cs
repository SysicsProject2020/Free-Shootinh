using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void destroyAll()
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
