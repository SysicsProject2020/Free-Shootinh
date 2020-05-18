using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaurusMagics : MonoBehaviour
{
    [Header("Magic:Shield")]
    public GameObject shield;

    [Header("Magic:DirectShot")]
    public GameObject DamageMissiles;
    public float speed;
    public void Magic1(int player)
    {
        if (player == 0)
        {
            GameObject ShieldInstantiated = Instantiate(shield, GameManagerPartie.instance.playerTowerPos, Quaternion.Euler(0, 0, 0));
            StartCoroutine(removeShield(ShieldInstantiated));
        }
        else
        {
            GameObject ShieldInstantiated = Instantiate(shield, GameManagerPartie.instance.enemyTowerPos, Quaternion.Euler(0, 0, 0));
            StartCoroutine(removeShield(ShieldInstantiated));
        }
    }
    IEnumerator removeShield(GameObject go)
    {
        yield return new WaitForSeconds(10);
        Destroy(go);
    }



    
    public void Magic2(int player)
    {

            StartCoroutine(launchingDamageMissile(player));


    }
    IEnumerator launchingDamageMissile(int player)
    {
        if (player == 0)
        {
            for (int k = 0; k < 5; k++)
            {
                yield return new WaitForSeconds(0.5f);
                Vector3 vo = Calculatevelocity(GameManagerPartie.instance.enemyTowerPos, GameManagerPartie.instance.playerTowerPos + new Vector3(5, 5, 5), speed);
                GameObject go = Instantiate(DamageMissiles, GameManagerPartie.instance.playerTowerPos + new Vector3(k * 2, 5, 5), DamageMissiles.transform.rotation);
                go.GetComponent<Rigidbody>().velocity = vo;
                go.GetComponent<bullet>().changedam(150);
                go.GetComponent<bullet>().sender = gameObject;
            }

        }
        else
        {
            for (int k = 0; k < 5; k++)
            {
                yield return new WaitForSeconds(0.5f);
                Vector3 vo = Calculatevelocity(GameManagerPartie.instance.playerTowerPos ,GameManagerPartie.instance.enemyTowerPos + new Vector3(5, 5, 5),  speed);
                GameObject go = Instantiate(DamageMissiles, GameManagerPartie.instance.enemyTowerPos + new Vector3(k * 2, 5, -5), DamageMissiles.transform.rotation);
                go.GetComponent<Rigidbody>().velocity = vo;
                go.GetComponent<bullet>().changedam(150);
                go.GetComponent<bullet>().sender = gameObject;
            }
        }


    }
    Vector3 Calculatevelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float sXZ = distanceXZ.magnitude;

        float Vxz = sXZ / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;

    }


}
