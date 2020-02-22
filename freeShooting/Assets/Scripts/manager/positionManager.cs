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
                        hoToShoot();
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
                        hoToShoot();
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
                        hoToShoot();
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
                        hoToShoot();
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
                        hoToShoot();
                        return true;
                    }
                    else return false;
                default: return false;
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
                        hoToShoot();
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
                        hoToShoot();
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
                        hoToShoot();
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
                        hoToShoot();
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
                        hoToShoot();
                        return true;
                    }
                    else return false;
                default: return false;
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
                        buildingTowerScript[0, 0] = null;
                        towerZone[0].GetComponent<BoxCollider>().enabled = true;
                        hoToShoot();
                        return true;
                    }
                    else return false;

                case -5:
                    if (buildingGameObject[0, 1] != null)
                    {
                        buildingGameObject[0, 1] = null;
                        buildingTowerScript[0, 1] = null;
                        towerZone[1].GetComponent<BoxCollider>().enabled = true;
                        hoToShoot();
                        return true;
                    }
                    else return false;

                case 5:
                    if (buildingGameObject[0, 2] != null)
                    {
                        buildingGameObject[0, 2] = null;
                        buildingTowerScript[0, 2] = null;
                        towerZone[2].GetComponent<BoxCollider>().enabled = true;
                        hoToShoot();
                        return true;
                    }
                    else return false;

                case 15:
                    if (buildingGameObject[0, 3] != null)
                    {
                        buildingGameObject[0, 3] = null;
                        buildingTowerScript[0, 3] = null;
                        towerZone[3].GetComponent<BoxCollider>().enabled = true;
                        hoToShoot();
                        return true;
                    }
                    else return false;

                case 25:
                    if (buildingGameObject[0, 4] != null)
                    {
                        buildingGameObject[0, 4] = null;
                        buildingTowerScript[0, 4] = null;
                        towerZone[4].GetComponent<BoxCollider>().enabled = true;
                        hoToShoot();
                        return true;
                    }
                    else return false;
                default: return false;
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
                        buildingTowerScript[1, 0] = null;
                        hoToShoot();
                        return true;
                    }
                    else return false;

                case -5:
                    if (buildingGameObject[1, 1] != null)
                    {
                        buildingGameObject[1, 1] = null;
                        buildingTowerScript[1, 1] = null;
                        hoToShoot();
                        return true;
                    }
                    else return false;

                case 5:
                    if (buildingGameObject[1, 2] != null)
                    {
                        buildingGameObject[1, 2] = null;
                        buildingTowerScript[1, 2] = null;
                        hoToShoot();
                        return true;
                    }
                    else return false;

                case 15:
                    if (buildingGameObject[1, 3] != null)
                    {
                        buildingGameObject[1, 3] = null;
                        buildingTowerScript[1, 3] = null;
                        hoToShoot();
                        return true;
                    }
                    else return false;

                case 25:
                    if (buildingGameObject[1, 4] != null)
                    {
                        buildingGameObject[1, 4] = null;
                        buildingTowerScript[1, 4] = null;
                        hoToShoot();
                        return true;
                    }
                    else return false;
                default: return false;
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

    public static void hoToShoot()
    {
        //player
        for (int j = 0; j < 5; j++)
        {
            if (buildingTowerScript[0, j] != null)
            {
                switch (buildingTowerScript[0, j].name)
                {
                    case "lazer tower":
                        //Debug.Log("lazer");
                        if (buildingGameObject[1, j] != null)
                        {
                            buildingGameObject[0, j].GetComponent<lazerShooting>().shoot(buildingGameObject[1, j]);
                        }
                        else
                        {
                            buildingGameObject[0, j].GetComponent<lazerShooting>().stopShoot();
                        }
                        break;

                    case "tesla":
                        Debug.Log("tesla");
                        if (buildingGameObject[1, j] != null)
                        {
                            buildingGameObject[0, j].GetComponent<teslashooting>().shoot(buildingGameObject[1, j]);
                        }
                        else
                        {
                            buildingGameObject[0, j].GetComponent<teslashooting>().StopShoot();
                        }
                        break;

                    case "healing tower":
                        Debug.Log("healing tower");
                        if (j - 1 != -1 && j + 1 != 5 && buildingGameObject[0, j + 1] != null && buildingGameObject[0, j - 1] != null)
                        {
                            buildingGameObject[0, j].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[0, j + 1], buildingGameObject[0, j - 1] });
                        }
                        else
                        {
                            if (j + 1 != 5 && buildingGameObject[0, j + 1] != null)
                            {
                                buildingGameObject[0, j].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[0, j + 1] });
                            }
                            else
                            {
                                if (j - 1 != -1 && buildingGameObject[0, j - 1] != null)
                                {
                                    buildingGameObject[0, j].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[0, j - 1] });
                                }
                                else
                                {
                                    buildingGameObject[0, j].GetComponent<healingTower>().stopHeal();
                                }
                            }

                        }
                        break;

                    case "canon":
                        Debug.Log("canon");
                        if (buildingGameObject[1, j] != null)
                        {
                            buildingGameObject[0, j].GetComponent<CanonTower>().shoot(buildingGameObject[1, j]);
                        }
                        else
                        {
                            if (j + 1 != 5 && buildingGameObject[1, j + 1] != null)
                            {
                                buildingGameObject[0, j].GetComponent<CanonTower>().shoot(buildingGameObject[1, j + 1]);
                            }
                            else
                            {
                                if (j - 1 != -1 && buildingGameObject[1, j - 1] != null)
                                {
                                    buildingGameObject[0, j].GetComponent<CanonTower>().shoot(buildingGameObject[1, j - 1]);
                                }
                                else
                                {
                                    buildingGameObject[0, j].GetComponent<CanonTower>().stopShoot();
                                }
                            }
                        }
                        break;

                    case "mortar":
                        Debug.Log("mortor");
                        int x = j;
                        bool goRight = true;

                        if (buildingGameObject[1, x] != null)
                        {
                            buildingGameObject[0, j].GetComponent<mortarShooting>().shoot(buildingGameObject[1, x]);
                        }
                        else
                        {
                            if (goRight)
                            {
                                x++;
                            }
                            else
                            {
                                x--;
                            }
                            if (x == 5)
                            {
                                x = j - 1;
                                goRight = false;
                            }

                            if (buildingGameObject[1, x] != null)
                            {
                                buildingGameObject[0, j].GetComponent<mortarShooting>().shoot(buildingGameObject[1, x]);
                            }
                            else
                            {
                                if (goRight)
                                {
                                    x++;
                                }
                                else
                                {
                                    x--;
                                }
                                if (x == 5)
                                {
                                    x = j - 1;
                                    goRight = false;
                                }
                                if (buildingGameObject[1, x] != null)
                                {
                                    buildingGameObject[0, j].GetComponent<mortarShooting>().shoot(buildingGameObject[1, x]);
                                }
                                else
                                {
                                    if (goRight)
                                    {
                                        x++;
                                    }
                                    else
                                    {
                                        x--;
                                    }
                                    if (x == 5)
                                    {
                                        x = j - 1;
                                        goRight = false;
                                    }
                                    if (buildingGameObject[1, x] != null)
                                    {
                                        buildingGameObject[0, j].GetComponent<mortarShooting>().shoot(buildingGameObject[1, x]);
                                    }
                                    else
                                    {
                                        if (goRight)
                                        {
                                            x++;
                                        }
                                        else
                                        {
                                            x--;
                                        }
                                        if (x == 5)
                                        {
                                            x = j - 1;
                                            goRight = false;
                                        }
                                        if (buildingGameObject[1, x] != null)
                                        {
                                            buildingGameObject[0, j].GetComponent<mortarShooting>().shoot(buildingGameObject[1, x]);
                                        }
                                        else
                                        {
                                            buildingGameObject[0, j].GetComponent<mortarShooting>().stopShoot();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "x-bow":
                        x = j;
                        goRight = true;

                        if (buildingGameObject[1, x] != null)
                        {
                            buildingGameObject[0, j].GetComponent<xbowShooting>().shoot(buildingGameObject[1, x]);
                        }
                        else
                        {
                            if (goRight)
                            {
                                x++;
                            }
                            else
                            {
                                x--;
                            }
                            if (x == 5)
                            {
                                x = j - 1;
                                goRight = false;
                            }

                            if (buildingGameObject[1, x] != null)
                            {
                                buildingGameObject[0, j].GetComponent<xbowShooting>().shoot(buildingGameObject[1, x]);
                            }
                            else
                            {
                                if (goRight)
                                {
                                    x++;
                                }
                                else
                                {
                                    x--;
                                }
                                if (x == 5)
                                {
                                    x = j - 1;
                                    goRight = false;
                                }
                                if (buildingGameObject[1, x] != null)
                                {
                                    buildingGameObject[0, j].GetComponent<xbowShooting>().shoot(buildingGameObject[1, x]);
                                }
                                else
                                {
                                    if (goRight)
                                    {
                                        x++;
                                    }
                                    else
                                    {
                                        x--;
                                    }
                                    if (x == 5)
                                    {
                                        x = j - 1;
                                        goRight = false;
                                    }
                                    if (buildingGameObject[1, x] != null)
                                    {
                                        buildingGameObject[0, j].GetComponent<xbowShooting>().shoot(buildingGameObject[1, x]);
                                    }
                                    else
                                    {
                                        if (goRight)
                                        {
                                            x++;
                                        }
                                        else
                                        {
                                            x--;
                                        }
                                        if (x == 5)
                                        {
                                            x = j - 1;
                                            goRight = false;
                                        }
                                        if (buildingGameObject[1, x] != null)
                                        {
                                            buildingGameObject[0, j].GetComponent<xbowShooting>().shoot(buildingGameObject[1, x]);
                                        }
                                        else
                                        {
                                            buildingGameObject[0, j].GetComponent<xbowShooting>().stopShoot();
                                        }
                                    }
                                }
                            }
                        }
                        break;

                }
            }
        }
        //enemy
        for (int j = 0; j < 5; j++)
        {
            if (buildingTowerScript[1, j] != null)
            {
                switch (buildingTowerScript[1, j].name)
                {
                    case "lazer tower":
                        //Debug.Log("lazer");
                        if (buildingGameObject[0, j] != null)
                        {
                            buildingGameObject[1, j].GetComponent<lazerShooting>().shoot(buildingGameObject[0, j]);
                        }
                        else
                        {
                            buildingGameObject[1, j].GetComponent<lazerShooting>().stopShoot();
                        }
                        break;

                    case "tesla":
                        Debug.Log("tesla");
                        if (buildingGameObject[0, j] != null)
                        {
                            buildingGameObject[1, j].GetComponent<teslashooting>().shoot(buildingGameObject[0, j]);
                        }
                        else
                        {
                            buildingGameObject[1, j].GetComponent<teslashooting>().StopShoot();
                        }
                        break;

                    case "healing tower":
                        Debug.Log("healing tower");
                        if (j - 1 != -1 && j + 1 != 5 && buildingGameObject[1, j + 1] != null && buildingGameObject[1, j - 1] != null)
                        {
                            buildingGameObject[1, j].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[1, j + 1], buildingGameObject[1, j - 1] });
                        }
                        else
                        {
                            if (j - 1 != 5 && buildingGameObject[1, j + 1] != null)
                            {
                                buildingGameObject[1, j].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[1, j + 1] });
                            }
                            else
                            {
                                if (j - 1 != -1 && buildingGameObject[1, j - 1] != null)
                                {
                                    buildingGameObject[1, j].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[1, j - 1] });
                                }
                                else
                                {
                                    buildingGameObject[1, j].GetComponent<healingTower>().stopHeal();
                                }
                            }

                        }
                        break;

                    case "canon":
                        Debug.Log("canon");
                        if (buildingGameObject[0, j] != null)
                        {
                            buildingGameObject[1, j].GetComponent<CanonTower>().shoot(buildingGameObject[0, j]);
                        }
                        else
                        {
                            if (j + 1 != 5 && buildingGameObject[0, j + 1] != null)
                            {
                                buildingGameObject[1, j].GetComponent<CanonTower>().shoot(buildingGameObject[0, j + 1]);
                            }
                            else
                            {
                                if (j - 1 != -1 && buildingGameObject[0, j - 1] != null)
                                {
                                    buildingGameObject[1, j].GetComponent<CanonTower>().shoot(buildingGameObject[0, j - 1]);
                                }
                                else
                                {
                                    buildingGameObject[1, j].GetComponent<CanonTower>().stopShoot();
                                }
                            }
                        }
                        break;

                    case "mortar":
                        Debug.Log("mortor");
                        int x = j;
                        bool goRight = true;

                        if (buildingGameObject[0, x] != null)
                        {
                            buildingGameObject[1, j].GetComponent<mortarShooting>().shoot(buildingGameObject[0, x]);
                        }
                        else
                        {
                            if (goRight)
                            {
                                x++;
                            }
                            else
                            {
                                x--;
                            }
                            if (x == 5)
                            {
                                x = j - 1;
                                goRight = false;
                            }

                            if (buildingGameObject[0, x] != null)
                            {
                                buildingGameObject[1, j].GetComponent<mortarShooting>().shoot(buildingGameObject[0, x]);
                            }
                            else
                            {
                                if (goRight)
                                {
                                    x++;
                                }
                                else
                                {
                                    x--;
                                }
                                if (x == 5)
                                {
                                    x = j - 1;
                                    goRight = false;
                                }
                                if (buildingGameObject[0, x] != null)
                                {
                                    buildingGameObject[1, j].GetComponent<mortarShooting>().shoot(buildingGameObject[0, x]);
                                }
                                else
                                {
                                    if (goRight)
                                    {
                                        x++;
                                    }
                                    else
                                    {
                                        x--;
                                    }
                                    if (x == 5)
                                    {
                                        x = j - 1;
                                        goRight = false;
                                    }
                                    if (buildingGameObject[0, x] != null)
                                    {
                                        buildingGameObject[1, j].GetComponent<mortarShooting>().shoot(buildingGameObject[0, x]);
                                    }
                                    else
                                    {
                                        if (goRight)
                                        {
                                            x++;
                                        }
                                        else
                                        {
                                            x--;
                                        }
                                        if (x == 5)
                                        {
                                            x = j - 1;
                                            goRight = false;
                                        }
                                        if (buildingGameObject[0, x] != null)
                                        {
                                            buildingGameObject[1, j].GetComponent<mortarShooting>().shoot(buildingGameObject[0, x]);
                                        }
                                        else
                                        {
                                            buildingGameObject[1, j].GetComponent<mortarShooting>().stopShoot();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "x-bow":
                        x = j;
                        goRight = true;

                        if (buildingGameObject[0, x] != null)
                        {
                            buildingGameObject[1, j].GetComponent<xbowShooting>().shoot(buildingGameObject[0, x]);
                        }
                        else
                        {
                            if (goRight)
                            {
                                x++;
                            }
                            else
                            {
                                x--;
                            }
                            if (x == 5)
                            {
                                x = j - 1;
                                goRight = false;
                            }

                            if (buildingGameObject[0, x] != null)
                            {
                                buildingGameObject[1, j].GetComponent<xbowShooting>().shoot(buildingGameObject[0, x]);
                            }
                            else
                            {
                                if (goRight)
                                {
                                    x++;
                                }
                                else
                                {
                                    x--;
                                }
                                if (x == 5)
                                {
                                    x = j - 1;
                                    goRight = false;
                                }
                                if (buildingGameObject[0, x] != null)
                                {
                                    buildingGameObject[1, j].GetComponent<xbowShooting>().shoot(buildingGameObject[0, x]);
                                }
                                else
                                {
                                    if (goRight)
                                    {
                                        x++;
                                    }
                                    else
                                    {
                                        x--;
                                    }
                                    if (x == 5)
                                    {
                                        x = j - 1;
                                        goRight = false;
                                    }
                                    if (buildingGameObject[0, x] != null)
                                    {
                                        buildingGameObject[1, j].GetComponent<xbowShooting>().shoot(buildingGameObject[0, x]);
                                    }
                                    else
                                    {
                                        if (goRight)
                                        {
                                            x++;
                                        }
                                        else
                                        {
                                            x--;
                                        }
                                        if (x == 5)
                                        {
                                            x = j - 1;
                                            goRight = false;
                                        }
                                        if (buildingGameObject[0, x] != null)
                                        {
                                            buildingGameObject[1, j].GetComponent<xbowShooting>().shoot(buildingGameObject[0, x]);
                                        }
                                        else
                                        {
                                            buildingGameObject[1, j].GetComponent<xbowShooting>().stopShoot();
                                        }
                                    }
                                }
                            }
                        }
                        break;

                }
            }
        }
    }
}
