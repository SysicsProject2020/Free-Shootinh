using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionManager : MonoBehaviour
{
    public static GameObject[,] buildingGameObject = new GameObject[2, 5];
    public static TowerScript[,] buildingTowerScript = new TowerScript[2, 5];
    public static GameObject[] towerZone = new GameObject[5];


    public GameObject[] towerZone_ = new GameObject[5];

    private void Start()
    {
        towerZone = towerZone_;
        

    }
    public static bool add(TowerScript tower, Vector3 place)
    {
        if (place.z < 0)
        {
            //player
            switch (place.x)
            {
                case -15:
                    if (buildingGameObject[0, 0] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 0] = go;
                        buildingTowerScript[0, 0] = tower;
                        towerZone[0].GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                case -5:
                    if (buildingGameObject[0, 1] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 1] = go;
                        buildingTowerScript[0, 1] = tower;
                        towerZone[1].GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                case 5:
                    if (buildingGameObject[0, 2] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 2] = go;
                        buildingTowerScript[0, 2] = tower;
                        towerZone[2].transform.GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                case 15:
                    if (buildingGameObject[0, 3] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 3] = go;
                        buildingTowerScript[0, 3] = tower;
                        towerZone[3].GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                case 25:
                    if (buildingGameObject[0, 4] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 4] = go;
                        buildingTowerScript[0, 4] = tower;
                        towerZone[4].GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                default:
                    return false;
            }
        }
        else
        {
            //enemy
            switch (place.x)
            {
                case -15:
                    if (buildingGameObject[0, 0] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 0] = go;
                        buildingTowerScript[1, 0] = tower;
                        return true;
                    }
                    else return false;

                case -5:
                    if (buildingGameObject[0, 1] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 1] = go;
                        buildingTowerScript[1, 1] = tower;
                        return true;
                    }
                    else return false;

                case 5:
                    if (buildingGameObject[1, 2] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 2] = go;
                        buildingTowerScript[1, 2] = tower;
                        return true;
                    }
                    else return false;

                case 15:
                    if (buildingGameObject[1, 3] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 3] = go;
                        buildingTowerScript[1, 3] = tower;
                        return true;
                    }
                    else return false;

                case 25:
                    if (buildingGameObject[1, 4] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 4] = go;
                        buildingTowerScript[1, 4] = tower;
                        return true;
                    }
                    else return false;

                default:
                    return false;
            }
        }
    }
    public static bool delete(Vector3 place)
    {
        if (place.z < 0)
        {
            switch (place.x)
            {
                case -15:
                    if (buildingGameObject[0, 0] != null)
                    {
                        buildingGameObject[0, 0] = null;
                        towerZone[0].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                case -5:
                    if (buildingGameObject[0, 1] != null)
                    {
                        buildingGameObject[0, 1] = null;
                        towerZone[1].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                case 5:
                    if (buildingGameObject[0, 2] != null)
                    {
                        buildingGameObject[0, 2] = null;
                        towerZone[2].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                case 15:
                    if (buildingGameObject[0, 3] != null)
                    {
                        buildingGameObject[0, 3] = null;
                        towerZone[3].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                case 25:
                    if (buildingGameObject[0, 4] != null)
                    {
                        buildingGameObject[0, 4] = null;
                        towerZone[4].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                default:
                    return false;
            }
        }
        else
        {
            switch (place.x)
            {
                case -15:
                    if (buildingGameObject[1, 0] != null)
                    {
                        buildingGameObject[1, 0] = null;
                        return true;
                    }
                    else return false;

                case -5:
                    if (buildingGameObject[1, 1] != null)
                    {
                        buildingGameObject[1, 1] = null;
                        return true;
                    }
                    else return false;

                case 5:
                    if (buildingGameObject[1, 2] != null)
                    {
                        buildingGameObject[1, 2] = null;
                        return true;
                    }
                    else return false;

                case 15:
                    if (buildingGameObject[1, 3] != null)
                    {
                        buildingGameObject[1, 3] = null;
                        return true;
                    }
                    else return false;

                case 25:
                    if (buildingGameObject[1, 4] != null)
                    {
                        buildingGameObject[1, 4] = null;
                        return true;
                    }
                    else return false;

                default:
                    return false;
            }
        }
        
    }
    private static void changeLayerMask(GameObject go, string layer)
    {
        go.layer = LayerMask.NameToLayer(layer);
        foreach (Transform g in go.transform)
        {
            g.gameObject.layer = LayerMask.NameToLayer(layer);
            foreach (Transform gobj in g.transform)
            {
                gobj.gameObject.layer = LayerMask.NameToLayer(layer);
            }
        }
    }

    private void Update()
    {
        for (int j = 0; j < 5; j++)
        {
            if (buildingTowerScript[0, j] !=null)
            {
                switch (buildingTowerScript[0, j].name)
                {                 
                    case "lazer tower":
                        
                        if (buildingGameObject[1, j] != null)
                        {
                            //Debug.Log("there is something");
                            buildingGameObject[0, j].GetComponent<lazerShooting>().shoot(buildingGameObject[1, j]);
                            //buildingTowerScript[0, j].prefab.GetComponent<lazerShooting>().shoot(buildingGameObject[1, j]);
                        }

                        break;
                }
            }
        }

    }
}
