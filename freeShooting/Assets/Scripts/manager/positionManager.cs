using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionManager : MonoBehaviour
{
    public static GameObject[,] building = new GameObject[2, 5];
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
                    if (building[0, 0] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        building[0, 0] = go;
                        towerZone[0].GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                case -5:
                    if (building[0, 1] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        building[0, 1] = go;
                        towerZone[1].GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                case 5:
                    if (building[0, 2] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        building[0, 2] = go;
                        towerZone[2].transform.GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                case 15:
                    if (building[0, 3] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        building[0, 3] = go;
                        towerZone[3].GetComponent<BoxCollider>().enabled = false;
                        return true;
                    }
                    else return false;

                case 25:
                    if (building[0, 4] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Player");
                        building[0, 4] = go;
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
                    if (building[0, 0] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        building[0, 0] = go;
                        return true;
                    }
                    else return false;

                case -5:
                    if (building[0, 1] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        building[0, 1] = go;
                        return true;
                    }
                    else return false;

                case 5:
                    if (building[1, 2] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        building[1, 2] = go;
                        return true;
                    }
                    else return false;

                case 15:
                    if (building[1, 3] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        building[1, 3] = go;
                        return true;
                    }
                    else return false;

                case 25:
                    if (building[1, 4] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, place, Quaternion.Euler(0, 0, 0));
                        changeLayerMask(go, "Enemy");
                        building[1, 4] = go;
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
                    if (building[0, 0] != null)
                    {
                        building[0, 0] = null;
                        towerZone[0].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                case -5:
                    if (building[0, 1] != null)
                    {
                        building[0, 1] = null;
                        towerZone[1].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                case 5:
                    if (building[0, 2] != null)
                    {
                        building[0, 2] = null;
                        towerZone[2].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                case 15:
                    if (building[0, 3] != null)
                    {
                        building[0, 3] = null;
                        towerZone[3].GetComponent<BoxCollider>().enabled = true;
                        return true;
                    }
                    else return false;

                case 25:
                    if (building[0, 4] != null)
                    {
                        building[0, 4] = null;
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
                    if (building[1, 0] != null)
                    {
                        building[1, 0] = null;
                        return true;
                    }
                    else return false;

                case -5:
                    if (building[1, 1] != null)
                    {
                        building[1, 1] = null;
                        return true;
                    }
                    else return false;

                case 5:
                    if (building[1, 2] != null)
                    {
                        building[1, 2] = null;
                        return true;
                    }
                    else return false;

                case 15:
                    if (building[1, 3] != null)
                    {
                        building[1, 3] = null;
                        return true;
                    }
                    else return false;

                case 25:
                    if (building[1, 4] != null)
                    {
                        building[1, 4] = null;
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
}
