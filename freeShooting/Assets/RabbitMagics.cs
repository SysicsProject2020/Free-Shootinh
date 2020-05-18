using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitMagics : MonoBehaviour
{
    [Header("Magic:CarrotShield")]
    public GameObject Carrot;
    public GameObject CarrotWall;
    [Header("Magic:MissilesMagic")]
    public GameObject Missiles;
    private GameObject MissilesInstantiated;
    // Start is called before the first frame update
    public void Magic2(int player)
    {
        if (player == 0)
        {


            GameObject go = MissilesInstantiated = Instantiate(Missiles);
            StartCoroutine(damagingAfterCarrotMissiles(MissilesInstantiated, player));
        }
        else
        {
            MissilesInstantiated = Instantiate(Missiles, Missiles.transform.position + new Vector3(0, 0, -45), Missiles.transform.rotation);
            StartCoroutine(damagingAfterCarrotMissiles(MissilesInstantiated, player));

        }

    }
    IEnumerator damagingAfterCarrotMissiles(GameObject go, int i)
    {
        yield return new WaitForSeconds(2.5f);
        if (i == 0)
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[1, j] != null)
                {
                    positionManager.buildingGameObject[1, j].GetComponent<target>().takeDamage(75);
                }
            }
            GameManagerPartie.instance.enemy_.GetComponent<target>().takeDamage(50);
            GameManagerPartie.instance.enemyTowerBase_.GetComponent<target>().takeDamage(75);
            Destroy(MissilesInstantiated);
        }
        else
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {
                    positionManager.buildingGameObject[0, j].GetComponent<target>().takeDamage(75);
                }
            }
            GameManagerPartie.instance.player_.GetComponent<target>().takeDamage(50);
            GameManagerPartie.instance.playerTowerBase_.GetComponent<target>().takeDamage(75);
            Destroy(MissilesInstantiated);
        }

    }



    // Update is called once per frame

    public void Magic1(int player)
    {
        if (player == 0)
        {
            GameObject CarrotWall_ = Instantiate(CarrotWall);
            Instantiate(Carrot, new Vector3(-7.4f, -9, -37.2f), Carrot.transform.rotation, CarrotWall_.transform);
            Instantiate(Carrot, new Vector3(7.4f, -9, -37.2f), Carrot.transform.rotation, CarrotWall_.transform);
            Instantiate(Carrot, new Vector3(-4f, -9, -33.7f), Carrot.transform.rotation, CarrotWall_.transform);
            Instantiate(Carrot, new Vector3(0, -9, -33.7f), Carrot.transform.rotation, CarrotWall_.transform);
            Instantiate(Carrot, new Vector3(4, -9, -33.7f), Carrot.transform.rotation, CarrotWall_.transform);
            for (int i = 0; i < 5; i++)
            {
                int time = Random.Range(2, 6);
                LeanTween.moveY(CarrotWall_.transform.GetChild(i).gameObject, 7.3f, time).setEaseLinear();
            }
            StartCoroutine(HideCarrot(CarrotWall_));
            StartCoroutine(DestroyCarrotWall(CarrotWall_));


        }
        else
        {
            GameObject CarrotWallEnemy_ = Instantiate(CarrotWall, CarrotWall.transform.position + new Vector3(0, 0, 60), Quaternion.Euler(0, 180, 0));
            Instantiate(Carrot, new Vector3(-7.4f, -9, 39), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            Instantiate(Carrot, new Vector3(7.4f, -9, 39), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            Instantiate(Carrot, new Vector3(-4f, -9, 34), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            Instantiate(Carrot, new Vector3(0, -9, 34), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            Instantiate(Carrot, new Vector3(4, -9, 34), Carrot.transform.rotation, CarrotWallEnemy_.transform);
            for (int i = 0; i < 5; i++)
            {
                int time = Random.Range(2, 6);
                LeanTween.moveY(CarrotWallEnemy_.transform.GetChild(i).gameObject, 7.3f, time).setEaseLinear();
            }
            StartCoroutine(HideCarrot(CarrotWallEnemy_));
            StartCoroutine(DestroyCarrotWall(CarrotWallEnemy_));



        }
    }
    IEnumerator HideCarrot(GameObject go)
    {
        yield return new WaitForSeconds(8);
        for (int i = 0; i < 5; i++)
        {
            LeanTween.moveY(go.transform.GetChild(i).gameObject, -9, 2).setEaseLinear();
        }

    }
    IEnumerator DestroyCarrotWall(GameObject go)
    {
        yield return new WaitForSeconds(10);
        Destroy(go);
    }
}
