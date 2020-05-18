using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PandaMagics : MonoBehaviour
{
    

    
    private bool testFree = false;
    private short currentCoins;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Magic1(int player)
    {
        if (player == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                GameManagerPartie.instance.itemParent.transform.GetChild(i).GetComponent<Button>().interactable = true;
                GameManagerPartie.instance.itemParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "0";
            }
            testFree = true;
            currentCoins = GameManagerPartie.instance.playerCoins;
        }
        else
        {

        }
    }
    void returnCardsValues(int player)
    {
        GameManagerPartie.instance.ChangeSprites();
        testFree = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (testFree)
        {
            if (currentCoins > GameManagerPartie.instance.playerCoins)
            {
                GameManagerPartie.instance.playerCoins = currentCoins;
                GameManagerPartie.instance.playerCoinsTxt.text = GameManagerPartie.instance.playerCoins.ToString();
                returnCardsValues(0);
            }
            else
                currentCoins = GameManagerPartie.instance.playerCoins;
        }
    }
    public void Magic2(int player)
    {
        if (player == 0)
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[0, j] != null)
                {
                    positionManager.buildingGameObject[0, j].gameObject.GetComponent<target>().gainhealth(150);
                }

            }
            GameManagerPartie.instance.player_.GetComponent<target>().gainhealth(50);
            GameManagerPartie.instance.playerTowerBase_.GetComponent<target>().gainhealth(200);
        }
        else
        {
            for (int j = 0; j < 5; j++)
            {
                if (positionManager.buildingGameObject[1, j] != null)
                {
                    positionManager.buildingGameObject[1, j].gameObject.GetComponent<target>().gainhealth(150);
                }

            }
            GameManagerPartie.instance.enemy_.GetComponent<target>().gainhealth(50);
            GameManagerPartie.instance.enemyTowerBase_.GetComponent<target>().gainhealth(200);
        }
    }
}
