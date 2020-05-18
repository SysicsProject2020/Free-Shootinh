using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMagics : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Magic:Destroy Tower")]
    public GameObject destroyMissile;
    [Header("Magic:Destroy All Towers")]
    public GameObject Explosion;
    private GameObject ExplosionInstatiated;
    public void Magic2()
    {
        ExplosionInstatiated = Instantiate(Explosion, new Vector3(0, 12, 0), Explosion.transform.rotation);
        StartCoroutine(destoryAllTowers(ExplosionInstatiated));
    }
    IEnumerator destoryAllTowers(GameObject go)
    {
        for (int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(Time.deltaTime * 10);
            go.transform.localScale += new Vector3(11, 10, 20);

        }
        short Pcoins = GameManagerPartie.instance.playerCoins;
        short Ecoins = GameManagerPartie.instance.enemyCoins;
        for (int k = 0; k < 2; k++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[k, j] != null)
                {
                    Destroy(positionManager.buildingGameObject[k, j]);
                    positionManager.delete(positionManager.buildingGameObject[k, j].transform.position);

                }
            }

        }
        GameManagerPartie.instance.playerCoins = Pcoins;
        GameManagerPartie.instance.enemyCoins = Ecoins;
        GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
        GameManagerPartie.instance.enemyCoinsTxt.text = GameManagerPartie.instance.enemyCoins.ToString();
        GameManagerPartie.instance.ChangeSprites();
        Destroy(go);


    }
    public void Magic1(int player)
    {
        if (player == 0)
        {
            int i = 0;
            int k = 0;
            short Pcoins = GameManagerPartie.instance.playerCoins;

            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[1, j] != null)
                {
                    i++;
                }
            }
            int rand = Random.Range(0, i - 1);

            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[1, j] != null)
                {

                    if (k == rand)
                    {
                        Instantiate(destroyMissile, new Vector3(positionManager.buildingGameObject[1, j].transform.position.x, positionManager.buildingGameObject[1, j].transform.position.y + 50, positionManager.buildingGameObject[1, j].transform.position.z), destroyMissile.transform.rotation);
                        StartCoroutine(DestroyTower(positionManager.buildingGameObject[1, j]));

                        positionManager.delete(positionManager.buildingGameObject[1, j].transform.position);
                    }
                    k++;
                }
            }
            GameManagerPartie.instance.playerCoins = Pcoins;
            GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
            GameManagerPartie.instance.ChangeSprites();

        }
        else
        {
            int i = 0;
            int k = 0;
            short Pcoins = GameManagerPartie.instance.enemyCoins;

            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {
                    i++;
                }
            }
            int rand = Random.Range(0, i - 1);

            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {

                    if (k == rand)
                    {
                        Instantiate(destroyMissile, new Vector3(positionManager.buildingGameObject[0, j].transform.position.x, positionManager.buildingGameObject[0, j].transform.position.y + 50, positionManager.buildingGameObject[0, j].transform.position.z), destroyMissile.transform.rotation);
                        StartCoroutine(DestroyTower(positionManager.buildingGameObject[0, j]));

                        positionManager.delete(positionManager.buildingGameObject[0, j].transform.position);
                    }
                    k++;
                }
            }
            GameManagerPartie.instance.enemyCoins = Pcoins;
            GameManagerPartie.instance.enemyCoinsTxt.text = GameManagerPartie.instance.enemyCoins.ToString();

        }
    }
    IEnumerator DestroyTower(GameObject go)
    {
        yield return new WaitForSeconds(2.2f);
        Destroy(go);

    }

}
