using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private bool shake = false;
    Vector2 startingPos;
    float speed = 10f; //how fast it shakes
    float amount = 10f;





    public GameObject inventory;
    private bool testClick=false;
    private int TowerNotSelectedClicked;
    
    public GameObject towerdetailsPanel;
    private GameManager gameManager;
   
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = this.GetComponent<GameManager>();
        startingPos.x = inventory.transform.position.x;
        startingPos.y = inventory.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (shake == true)
        {
            inventory.transform.position = new Vector2(startingPos.x + (Mathf.Sin(Time.time * speed) * amount), startingPos.y + (Mathf.Sin(Time.time * speed) * amount));
        }
    }
    public void OnUseClick()
    {
        towerdetailsPanel.SetActive(false);
        testClick = true;
        shake = true;
    }
    public void OnclickInventory(int i)
    {
        
         if(testClick==true)
        {
            gameManager.SwitchTowers(i, TowerNotSelectedClicked);
            gameManager.fillSprites();
            shake = false;
            inventory.transform.position = startingPos;
            testClick = false;
        }
        else
        {
            towerdetailsPanel.SetActive(true);
            TowerScript[] towers = gameManager.GetSelectedTowers();
            GameObject desc = towerdetailsPanel.transform.Find("description").gameObject;
            TMPro.TextMeshProUGUI txt = desc.GetComponent<TMPro.TextMeshProUGUI>();
            txt.text = "Tower Name : " + towers[i].name;
            towerdetailsPanel.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = false;
        }
    }
    public void OnChoosingCards(int i)
    {
        towerdetailsPanel.SetActive(true);
        TowerScript [] towers= gameManager.GetNonSelectedTowers();
        GameObject desc = towerdetailsPanel.transform.Find("description").gameObject;
        TMPro.TextMeshProUGUI txt = desc.GetComponent<TMPro.TextMeshProUGUI>();
        txt.text = "Tower Name : " + towers[i].name ;
        towerdetailsPanel.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = true;
        TowerNotSelectedClicked = i;
    }
}
