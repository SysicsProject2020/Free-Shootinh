using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuManager : MonoBehaviour
{    
    public GameObject mainPanel;
    public GameObject settingPanel;
    public GameObject inventoryPanel;
    public GameObject shopPanel;
    public GameObject SelectedTowersPanel;
    public GameObject NotSelectedTowersPanel;
    public GameObject towerButton;
    public GameObject HerosButton;
    public GameObject HerosPanel;
    public GameObject GunsPanel;
    public GameObject GunButton;

    public GameObject unselectedTowerButton; 
    public GameObject unselectedPlayerButton;
    public GameObject unselectedGunButton;
    public GameObject selectedTowerButton;
    public GameObject selectedPlayerButton;
    public GameObject selectedGunButton;
    public GameObject towerPanel;
    public GameObject playerPanel;
    public GameObject gunpanel;

    private void Start()
    {
        towersMenuInstantiate();
        fillTowersSprites();
        playerMenuInstantiate();
        ShopMenuInstantiate();
    }

    public void tower()
    {
        unselectedTowerButton.SetActive(false);
        unselectedPlayerButton.SetActive(true);
        unselectedGunButton.SetActive(true);
        selectedTowerButton.SetActive(true);
        selectedPlayerButton.SetActive(false);
        selectedGunButton.SetActive(false);
        towerPanel.SetActive(true);
        playerPanel.SetActive(false);
        gunpanel.SetActive(false);
    }
    public void player()
    {
        unselectedTowerButton.SetActive(true);
        unselectedPlayerButton.SetActive(false);
        unselectedGunButton.SetActive(true);
        selectedTowerButton.SetActive(false);
        selectedPlayerButton.SetActive(true);
        selectedGunButton.SetActive(false);
        towerPanel.SetActive(false);
        playerPanel.SetActive(true);
        gunpanel.SetActive(false);
    }
    public void gun()
    {
        unselectedTowerButton.SetActive(true);
        unselectedPlayerButton.SetActive(true);
        unselectedGunButton.SetActive(false);
        selectedTowerButton.SetActive(false);
        selectedPlayerButton.SetActive(false);
        selectedGunButton.SetActive(true);
        towerPanel.SetActive(false);
        playerPanel.SetActive(false);
        gunpanel.SetActive(true);
    }
    
    public void playPvm()
    {
        SceneManager.LoadScene("pvm");
    }
    public void setting()
    {      
        settingPanel.SetActive(true);       
    }
    public void exitSetting()
    {
        settingPanel.SetActive(false);
    }
    public void shop()
    {        
        shopPanel.SetActive(true);
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }
    public void main()
    {
        shopPanel.SetActive(false);
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }
    public void inventory()
    {
        shopPanel.SetActive(false);
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }
    public void fillTowersSprites()
    {
        for (int i = 0; i < 6; i++)
        {
            SelectedTowersPanel.transform.GetChild(i).GetComponentInChildren<Image>().sprite = GameManager.instance.GetSelectedTowers()[i].image;
        }
        for (int j = 0; j < GameManager.instance.GetNonSelectedTowers().Length; j++)
        {

            NotSelectedTowersPanel.transform.GetChild(j).GetComponentInChildren<Image>().sprite = GameManager.instance.GetNonSelectedTowers()[j].image;
        }
    }
    public void towersMenuInstantiate()
    {

        for (int j = 0; j < GameManager.instance.GetNonSelectedTowers().Length; j++)
        {
            GameObject go = Instantiate(towerButton, NotSelectedTowersPanel.transform);
            RegisterListenerTowerSwitch(go, j);
        }
    }
    public void RegisterListenerTowerSwitch(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => { MenuManager1.instance.OnChoosingCards(i); });
    }
    void playerMenuInstantiate()
    {
        for (int i = 0; i < GameManager.instance.players.Length; i++)
        {
            GameObject go = Instantiate(HerosButton, HerosPanel.transform) as GameObject;

            HerosPanel.transform.GetChild(i).GetComponentInChildren<Image>().sprite = GameManager.instance.players[i].image;
            RegisterListener(go, i);
        }


    }
    public void RegisterListener(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
       // myButton.onClick.AddListener(() => { OnPlayerClick(i); });

    }
    void ShopMenuInstantiate()
    {
        for (int i = 0; i < GameManager.instance.guns.Length; i++)
        {
            GameObject go = Instantiate(GunButton, GunsPanel.transform) as GameObject;

            GunsPanel.transform.GetChild(i).GetComponentInChildren<Image>().sprite = GameManager.instance.guns[i].image;
            RegisterListenerShop(go, i);
        }

    }

    public void RegisterListenerShop(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        //myButton.onClick.AddListener(() => { OnGunClick(i); });

    }
}

