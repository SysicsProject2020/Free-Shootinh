using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionManager : MonoBehaviour
{
    public static GameObject[,] buildingGameObject = new GameObject[2, 5];
    public static TowerScript[,] buildingTowerScript = new TowerScript[2, 5];
    public static GameObject[] towerZone = new GameObject[5];
    public GameObject[] towerZone_ = new GameObject[5];
    public static byte numBuildedTowers = 0;

    private void Start()
    {
        towerZone = towerZone_;
    }
    public static void add(TowerScript tower, Vector3 place, byte lvl = 0)
    {
        if (place.z < 0)
        {
            //player
            switch (place.x)
            {
                case -16.52f:
                    if (buildingGameObject[0, 0] == null)
                    {

                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 0, 0));
                        Instantiate(GameManager.instance.bases[tower.level - 1], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 0] = go;
                        buildingTowerScript[0, 0] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_player());
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_player());
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_player());
                        towerZone[0].GetComponent<BoxCollider>().enabled = false;
                        PlayerShoot(0);
                        addCoinPlayer((short)-(int)buildingTowerScript[0, 0].cost);
                        HealingTowerAdd0(0);
                        aiCanBuild();
                    }
                    break;

                case -8.27f:
                    if (buildingGameObject[0, 1] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 0, 0));
                        Instantiate(GameManager.instance.bases[tower.level - 1], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 1] = go;
                        buildingTowerScript[0, 1] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_player());
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_player());
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_player());
                        towerZone[1].GetComponent<BoxCollider>().enabled = false;
                        PlayerShoot(1);
                        addCoinPlayer((short)-(int)buildingTowerScript[0, 1].cost);
                        HealingTowerAdd1(0);
                        aiCanBuild();
                    }
                    break;

                case 0:
                    if (buildingGameObject[0, 2] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 0, 0));
                        Instantiate(GameManager.instance.bases[tower.level - 1], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 2] = go;
                        buildingTowerScript[0, 2] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_player());
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_player());
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_player());
                        towerZone[2].transform.GetComponent<BoxCollider>().enabled = false;
                        PlayerShoot(2);
                        addCoinPlayer((short)-(int)buildingTowerScript[0, 2].cost);

                        HealingTowerAdd2(0);
                        aiCanBuild();
                    }
                    break;

                case 8.27f:
                    if (buildingGameObject[0, 3] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 0, 0));
                        Instantiate(GameManager.instance.bases[tower.level - 1], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 3] = go;
                        buildingTowerScript[0, 3] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_player());
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_player());
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_player());
                        towerZone[3].GetComponent<BoxCollider>().enabled = false;
                        PlayerShoot(3);
                        addCoinPlayer((short)-(int)buildingTowerScript[0, 3].cost);

                        HealingTowerAdd3(0);
                        aiCanBuild();
                    }
                    break;

                case 16.52f:
                    if (buildingGameObject[0, 4] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 0, 0));
                        Instantiate(GameManager.instance.bases[tower.level - 1], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Player");
                        buildingGameObject[0, 4] = go;
                        buildingTowerScript[0, 4] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_player());
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_player());
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_player());
                        towerZone[4].GetComponent<BoxCollider>().enabled = false;
                        PlayerShoot(4);
                        addCoinPlayer((short)-(int)buildingTowerScript[0, 4].cost);
                        HealingTowerAdd4(0);
                        aiCanBuild();
                    }
                    break;
            }

            for (int i = 0; i < 5; i++)
            {
                EnemyShoot(i);
            }

        }
        else
        {
            //enemy
            switch (place.x)
            {
                case -16.52f:
                    if (buildingGameObject[1, 0] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 180, 0));
                        Instantiate(GameManager.instance.bases[lvl], place, Quaternion.Euler(0, 0, 0),go.transform);
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 0] = go;
                        buildingTowerScript[1, 0] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_enemy(lvl));
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_enemy(lvl));
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_enemy(lvl));
                        EnemyShoot(0);
                        HealingTowerAdd0(1);
                        addCoinEnemy((short)-(int)buildingTowerScript[1, 0].cost);
                        numBuildedTowers++;
                    }
                    break;

                case -8.27f:
                    if (buildingGameObject[1, 1] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 180, 0));
                        Instantiate(GameManager.instance.bases[lvl], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 1] = go;
                        buildingTowerScript[1, 1] = tower;
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_enemy(lvl));
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_enemy(lvl));
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_enemy(lvl));
                        EnemyShoot(1);
                        HealingTowerAdd1(1);
                        addCoinEnemy((short)-(int)buildingTowerScript[1, 1].cost);
                        numBuildedTowers++;
                    }
                    break;

                case 0:
                    if (buildingGameObject[1, 2] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 180, 0));
                        Instantiate(GameManager.instance.bases[lvl], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 2] = go;
                        buildingTowerScript[1, 2] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_enemy(lvl));
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_enemy(lvl));
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_enemy(lvl));
                        EnemyShoot(2);
                        HealingTowerAdd2(1);
                        addCoinEnemy((short)-(int)buildingTowerScript[1, 2].cost);
                        numBuildedTowers++;
                    }
                    break;

                case 8.27f:
                    if (buildingGameObject[1, 3] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 180, 0));
                        Instantiate(GameManager.instance.bases[lvl], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 3] = go;
                        buildingTowerScript[1, 3] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_enemy(lvl));
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_enemy(lvl));
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_enemy(lvl));
                        EnemyShoot(3);
                        HealingTowerAdd3(1);
                        addCoinEnemy((short)-(int)buildingTowerScript[1, 3].cost);
                        numBuildedTowers++;
                    }
                    break;

                case 16.52f:
                    if (buildingGameObject[1, 4] == null)
                    {
                        GameObject go = Instantiate(tower.prefab, new Vector3(place.x, tower.prefab.transform.position.y, place.z), Quaternion.Euler(0, 180, 0));
                        Instantiate(GameManager.instance.bases[lvl], place, Quaternion.Euler(0, 0, 0), go.transform);
                        changeLayerMask(go, "Enemy");
                        buildingGameObject[1, 4] = go;
                        buildingTowerScript[1, 4] = tower;
                        go.GetComponent<towerInf>().SetDamage(tower.Get_damage_enemy(lvl));
                        go.GetComponent<towerInf>().SetHealth(tower.Get_health_enemy(lvl));
                        go.GetComponent<towerInf>().SetFireRate(tower.Get_fireRate_enemy(lvl));
                        EnemyShoot(4);
                        HealingTowerAdd4(1);
                        addCoinEnemy((short)-(int)buildingTowerScript[1, 4].cost);
                        numBuildedTowers++;
                    }
                    break;
            }
            for (int i = 0; i < 5; i++)
            {
                PlayerShoot(i);
            }
        }

    }
    public static void delete(Vector3 place)
    {
        if (place.z < 0)
        {
            switch (place.x)
            {
                case -16.52f:
                    if (buildingGameObject[0, 0] != null)
                    {
                        addCoinEnemy((short)(buildingTowerScript[0, 0].cost - 50));
                        buildingGameObject[0, 0] = null;
                        buildingTowerScript[0, 0] = null;
                        towerZone[0].GetComponent<BoxCollider>().enabled = true;
                        HealingTowerDelete0(0);
                        aiCanBuild();
                    }
                    break;

                case -8.27f:
                    if (buildingGameObject[0, 1] != null)
                    {
                        addCoinEnemy((short)(buildingTowerScript[0, 1].cost - 50));
                        buildingGameObject[0, 1] = null;
                        buildingTowerScript[0, 1] = null;
                        towerZone[1].GetComponent<BoxCollider>().enabled = true;
                        HealingTowerDelete1(0);
                        aiCanBuild();
                    }
                    break;

                case 0:
                    if (buildingGameObject[0, 2] != null)
                    {
                        addCoinEnemy((short)(buildingTowerScript[0, 2].cost - 50));
                        buildingGameObject[0, 2] = null;
                        buildingTowerScript[0, 2] = null;
                        towerZone[2].GetComponent<BoxCollider>().enabled = true;
                        HealingTowerDelete2(0);
                        aiCanBuild();
                    }
                    break;

                case 8.17f:
                    if (buildingGameObject[0, 3] != null)
                    {
                        addCoinEnemy((short)(buildingTowerScript[0, 3].cost - 50));
                        buildingGameObject[0, 3] = null;
                        buildingTowerScript[0, 3] = null;
                        towerZone[3].GetComponent<BoxCollider>().enabled = true;
                        HealingTowerDelete3(0);
                        aiCanBuild();
                    }
                    break;

                case 16.52f:
                    if (buildingGameObject[0, 4] != null)
                    {
                        addCoinEnemy((short)(buildingTowerScript[0, 4].cost - 50));
                        buildingGameObject[0, 4] = null;
                        buildingTowerScript[0, 4] = null;
                        towerZone[4].GetComponent<BoxCollider>().enabled = true;
                        HealingTowerDelete4(0);
                        aiCanBuild();
                    }
                    break;
            }
            for (int i = 0; i < 5; i++)
            {
                EnemyShoot(i);
            }

        }
        else
        {   
            switch (place.x)
            {
                case -16.52f:
                    if (buildingGameObject[1, 0] != null)
                    {
                        addCoinPlayer((short)(buildingTowerScript[1, 0].cost - 50));
                        buildingGameObject[1, 0] = null;
                        buildingTowerScript[1, 0] = null;
                        HealingTowerDelete0(1);
                        numBuildedTowers--;
                       // aiCanBuild();
                    }
                    break;

                case -8.27f:
                    if (buildingGameObject[1, 1] != null)
                    {
                        addCoinPlayer((short)(buildingTowerScript[1, 1].cost - 50));
                        buildingGameObject[1, 1] = null;
                        buildingTowerScript[1, 1] = null;
                        HealingTowerDelete1(1);
                        numBuildedTowers--;
                       // aiCanBuild();
                    }
                    break;

                case 0:
                    if (buildingGameObject[1, 2] != null)
                    {
                        addCoinPlayer((short)(buildingTowerScript[1, 2].cost - 50));
                        buildingGameObject[1, 2] = null;
                        buildingTowerScript[1, 2] = null;
                        HealingTowerDelete2(1);
                        numBuildedTowers--;
                        //aiCanBuild();
                    }
                    break;

                case 8.27f:
                    if (buildingGameObject[1, 3] != null)
                    {
                        addCoinPlayer((short)(buildingTowerScript[1, 3].cost - 50));
                        buildingGameObject[1, 3] = null;
                        buildingTowerScript[1, 3] = null;
                        HealingTowerDelete3(1);
                        numBuildedTowers--;
                       // aiCanBuild();
                    }
                    break;

                case 16.52f:
                    if (buildingGameObject[1, 4] != null)
                    {
                        addCoinPlayer((short)(buildingTowerScript[1, 4].cost - 50));
                        buildingGameObject[1, 4] = null;
                        buildingTowerScript[1, 4] = null;
                        HealingTowerDelete4(1);
                        numBuildedTowers--;
                       // aiCanBuild();
                    }
                    break;
            }

            for (int i = 0; i < 5; i++)
            {
                PlayerShoot(i);
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

    public static void PlayerShoot(int j)
    {
        if (buildingTowerScript[0, j] != null)
        {
            switch (buildingTowerScript[0, j].name)
            {
                case "block tower":
                    break;

                case "mirror tower":
                    break;

                case "freezing tower":
                    //Debug.Log("lazer");
                    if (buildingGameObject[1, j] != null)
                    {
                        buildingGameObject[0, j].GetComponent<freezingTower>().shoot(buildingGameObject[1, j]);
                    }
                    else
                    {
                        buildingGameObject[0, j].GetComponent<freezingTower>().unfreeze();
                    }
                    break;

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
                    //Debug.Log("tesla");
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
                    //Debug.Log("healing tower");
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
                    //Debug.Log("canon");
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
                    //Debug.Log("mortor");
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

    public static void EnemyShoot(int j)
    {
        if (buildingTowerScript[1, j] != null)
        {
            switch (buildingTowerScript[1, j].name)
            {
                case "block tower":
                    break;

                case "mirror tower":
                    break;

                case "freezing tower":
                    if (buildingGameObject[0, j] != null)
                    {
                        buildingGameObject[1, j].GetComponent<freezingTower>().shoot(buildingGameObject[0, j]);
                    }
                    else
                    {
                        buildingGameObject[1, j].GetComponent<freezingTower>().unfreeze();
                    }
                    break;
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
                    //Debug.Log("tesla");
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
                    //Debug.Log("healing tower");
                    if (j - 1 != -1 && j + 1 != 5 && buildingGameObject[1, j + 1] != null && buildingGameObject[1, j - 1] != null)
                    {
                        buildingGameObject[1, j].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[1, j + 1], buildingGameObject[1, j - 1] });
                    }
                    else
                    {
                        if (j + 1 != 5 && buildingGameObject[1, j + 1] != null)
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
                    //Debug.Log("canon");
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
                    //Debug.Log("mortor");
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

    private static void addCoinEnemy(short a)
    {
        GameManagerPartie.instance.enemyCoins += a;
        GameManagerPartie.instance.enemyCoinsTxt.text = GameManagerPartie.instance.enemyCoins.ToString();
        GameManagerPartie.instance.ChangeInteractableSpritesPrice();
    }
    private static void addCoinPlayer(short a)
    {
        GameManagerPartie.instance.playerCoins += a;
        GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
        GameManagerPartie.instance.ChangeInteractableSpritesPrice();
    }
    public static void aiCanBuild()
    {
        if (AIeasy.CurrentState != AIeasy.AIState.start)
        {
            if (numBuildedTowers < 5)
            {
                if (AIeasy.towers[AIeasy.minCostTower].cost <= GameManagerPartie.instance.enemyCoins)
                {
                    Debug.Log("aiCanBuild  aistate : build");
                    AIeasy.CurrentState = AIeasy.AIState.build;
                } 
            }     
        }
    }

    public static void HealingTowerAdd0(byte i)
    {
        if (buildingTowerScript[i, 1] != null && buildingTowerScript[i, 1].name == "healing tower")
        {
            if (buildingGameObject[i, 2] != null)
            {
                buildingGameObject[i, 1].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 2], buildingGameObject[i, 0] });
            }
            else
            {
                buildingGameObject[i, 1].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 0] });
            }
        }
    }
    public static void HealingTowerAdd1(byte i)
    {
        if (buildingTowerScript[i, 2] != null && buildingTowerScript[i, 2].name == "healing tower")
        {
            if (buildingGameObject[i, 3] != null)
            {
                buildingGameObject[i, 2].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 3], buildingGameObject[i, 1] });
            }
            else
            {
                buildingGameObject[i, 2].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 1] });
            }
        }
        if (buildingTowerScript[i, 0] != null && buildingTowerScript[i, 0].name == "healing tower")
        {
            buildingGameObject[i, 0].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 1] });
        }
    }
    public static void HealingTowerAdd2(byte i)
    {
        if (buildingTowerScript[i, 1] != null && buildingTowerScript[i, 1].name == "healing tower")
        {
            if (buildingGameObject[i, 0] != null)
            {
                buildingGameObject[i, 1].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 2], buildingGameObject[i, 0] });
            }
            else
            {
                buildingGameObject[i, 1].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 2] });
            }
        }
        if (buildingTowerScript[i, 3] != null && buildingTowerScript[i, 3].name == "healing tower")
        {
            if (buildingGameObject[i, 4] != null)
            {
                buildingGameObject[i, 3].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 2], buildingGameObject[i, 4] });
            }
            else
            {
                buildingGameObject[i, 3].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 2] });
            }
        }
    }
    public static void HealingTowerAdd3(byte i)
    {
        if (buildingTowerScript[i, 2] != null && buildingTowerScript[i, 2].name == "healing tower")
        {
            if (buildingGameObject[i, 1] != null)
            {
                buildingGameObject[i, 2].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 3], buildingGameObject[i, 1] });
            }
            else
            {
                buildingGameObject[i, 2].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 3] });
            }
        }
        if (buildingTowerScript[i, 4] != null && buildingTowerScript[i, 4].name == "healing tower")
        {
            buildingGameObject[i, 4].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 3] });
        }
    }
    public static void HealingTowerAdd4(byte i)
    {
        if (buildingTowerScript[i, 3] != null && buildingTowerScript[i, 3].name == "healing tower")
        {
            if (buildingGameObject[i, 2] != null)
            {
                buildingGameObject[i, 3].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 2], buildingGameObject[i, 4] });
            }
            else
            {
                buildingGameObject[i, 3].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 4] });
            }
        }
    }

    public static void HealingTowerDelete0(byte i)
    {
        if (buildingTowerScript[i, 1] != null && buildingTowerScript[i, 1].name == "healing tower")
        {
            if (buildingGameObject[i, 2] != null)
            {
                buildingGameObject[i, 1].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 2] });
            }
        }
    }
    public static void HealingTowerDelete1(byte i)
    {
        if (buildingTowerScript[i, 2] != null && buildingTowerScript[i, 2].name == "healing tower")
        {
            if (buildingGameObject[i, 3] != null)
            {
                buildingGameObject[i, 2].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 3] });
            }
        }
        if (buildingTowerScript[i, 0] != null && buildingTowerScript[i, 0].name == "healing tower")
        {
            buildingGameObject[i, 0].GetComponent<healingTower>().heal(new GameObject[] { });
        }
    }
    public static void HealingTowerDelete2(byte i)
    {
        if (buildingTowerScript[i, 1] != null && buildingTowerScript[i, 1].name == "healing tower")
        {
            if (buildingGameObject[i, 0] != null)
            {
                buildingGameObject[i, 1].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 0] });
            }
        }
        if (buildingTowerScript[i, 3] != null && buildingTowerScript[i, 3].name == "healing tower")
        {
            if (buildingGameObject[i, 4] != null)
            {
                buildingGameObject[i, 3].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 4] });
            }
        }
    }
    public static void HealingTowerDelete3(byte i)
    {
        if (buildingTowerScript[i, 2] != null && buildingTowerScript[i, 2].name == "healing tower")
        {
            if (buildingGameObject[i, 1] != null)
            {
                buildingGameObject[i, 2].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 1] });
            }
        }
        if (buildingTowerScript[i, 4] != null && buildingTowerScript[i, 4].name == "healing tower")
        {
            buildingGameObject[i, 4].GetComponent<healingTower>().heal(new GameObject[] { });
        }
    }
    public static void HealingTowerDelete4(byte i)
    {
        if (buildingTowerScript[i, 3] != null && buildingTowerScript[i, 3].name == "healing tower")
        {
            if (buildingGameObject[i, 2] != null)
            {
                buildingGameObject[i, 3].GetComponent<healingTower>().heal(new GameObject[] { buildingGameObject[i, 2] });
            }
        }
    }
}
