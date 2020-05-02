﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuManager1 : MonoBehaviour
{
    public static MenuManager1 instance;

    private bool shake = false;
    Vector2 startingPos;
    float speed = 10f; //how fast it shakes
    float amount = 10f;
    private int playerClicked = 0;
    private byte GunClicked;
    public TextMeshProUGUI DiamondTxt;
    public GameObject XPBar;
    public GameObject AreYouSureTowersPanel;
    public Button backMainFromInventoryBtn;
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject inventoryPanel;
    public GameObject characterPanel;
    public GameObject shopPanel;
    public GameObject SelectGameDiffPanel;
    public Button backMainFromcharacterBtn;
    public Button backMainFromShopBtn;
    public GameObject PlayerdetailsPanel;
    public GameObject PlayerSlot;
    public GameObject PlayerSelection;
    public GameObject towerSlot;
    public GameObject GunSlot;
    public GameObject ShopSelection;
    public GameObject GunDetailsPanel;
    public GameObject inventory;
    private bool testClick=false;
    private int TowerNotSelectedClicked;
    private int TowerSelectedClicked;
    public GameObject towerdetailsPanel;
    public GameObject towerNotSelectedMenu;
    private TowerScript[] towersSelected = new TowerScript[6];
    private TowerScript[] towersNotSelected;
    private TMPro.TextMeshProUGUI txtTowerDetails;
    private TMPro.TextMeshProUGUI txtPlayerDetails;
    private TMPro.TextMeshProUGUI txtGunDetails;
    private TMPro.TextMeshProUGUI UnlockDetails;


    public GameObject upgradePlayersBtn;
    public GameObject upgradeGunBtn;
    public GameObject UnlockPlayerBtn;
    public GameObject UnlockGunBtn;
    public GameObject UsePlayerButton;
    public GameObject UseGunButton;


    public GameObject NotEnoughDiamonds;
    public GameObject Congrats;


    public GameObject UnlockTowerBtn;
    public GameObject useTowerButton;
    public GameObject upgradeTowersBtn;
    public GameObject upgradeTowersNotSelectedBtn;


    private void Awake()
    {
        instance = this;
        towersNotSelected = GameManager.instance.GetNonSelectedTowers();
        towersSelected = GameManager.instance.GetSelectedTowers();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        DiamondTxt.text = GameManager.instance.diamond + "Diamonds";
        txtTowerDetails = towerdetailsPanel.transform.Find("description").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        txtPlayerDetails = PlayerdetailsPanel.transform.Find("description").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        txtGunDetails = GunDetailsPanel.transform.Find("description").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        UnlockDetails = AreYouSureTowersPanel.transform.Find("description").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        startingPos.x = inventory.transform.position.x;
        startingPos.y = inventory.transform.position.y;
        playerMenuInstantiate();
        towersMenuInstantiate();
        ShopMenuInstantiate();
        fillSprites();

        //  Debug.Log(GameManager.instance.getPlayer().name);
    }

    // Update is called once per frame
    void Update()
    {
        if (shake == true)
        {
 
            inventory.transform.position = new Vector2(startingPos.x + (Mathf.Sin(Time.time * speed) * amount), startingPos.y + (Mathf.Sin(Time.time * speed) * amount));
            
        }

        
    }
    public void IgnoreClick()
    {
        if(testClick==true)
        {
            testClick = false;
            shake = false;
            inventory.transform.position = startingPos;
        }
    }
    void playerMenuInstantiate()
    {
        for (int i = 0; i < GameManager.instance.players.Length; i++)
        {
            GameObject go = Instantiate(PlayerSlot, PlayerSelection.transform.position, Quaternion.identity) as GameObject;

            go.transform.SetParent(PlayerSelection.transform);
            GameObject ChildGameObject1 = go.transform.GetChild(0).gameObject;
            GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
            ChildGameObject2.GetComponent<Image>().sprite = GameManager.instance.players[i].image;
            RegisterListener(ChildGameObject1, i);
        }

        
    }
    void ShopMenuInstantiate()
    {
        for (int i = 0; i < GameManager.instance.guns.Length; i++)
        {
            GameObject go = Instantiate(GunSlot, ShopSelection.transform.position, Quaternion.identity) as GameObject;

            go.transform.SetParent(ShopSelection.transform);
            GameObject ChildGameObject1 = go.transform.GetChild(0).gameObject;
            GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
            ChildGameObject2.GetComponent<Image>().sprite = GameManager.instance.guns[i].image;
            RegisterListenerShop(ChildGameObject1, i);
        }

    }

    public void RegisterListenerShop(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => { OnGunClick(i); });

    }
    public void RegisterListener(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => {OnPlayerClick(i); });

    }
    public void towersMenuInstantiate()
    {

        for (int j = 0; j < towersNotSelected.Length; j++)
        {
            GameObject go = Instantiate(towerSlot,towerNotSelectedMenu.transform);
            RegisterListenerTowerSwitch(go, j);
        }
    }
    public void RegisterListenerTowerSwitch(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => { MenuManager1.instance.OnChoosingCards(i); });
    }
    public void fillSprites()
    {
        for (int i = 0; i < 6; i++)
        {
             inventory.transform.GetChild(i).GetComponentInChildren<Image>().sprite = GameManager.instance.GetSelectedTowers()[i].image;
        }
        for (int j = 0; j < towersNotSelected.Length; j++)
        {
            Debug.Log(towersNotSelected[j].name);
            towerNotSelectedMenu.transform.GetChild(j).GetComponentInChildren<Image>().sprite = towersNotSelected[j].image;
        }
    }
    public void OnUseClick()
    {
        towerdetailsPanel.SetActive(false);
        testClick = true;
        shake = true;
        backMainFromInventoryBtn.GetComponent<Button>().interactable = true;
    
    }
    public void OnclickInventory(int i)
    {
        
         if (testClick==true)
        {
            SwitchTowers(i, TowerNotSelectedClicked);
            fillSprites();
            shake = false;
            inventory.transform.position = startingPos;
            testClick = false;
        }
        else
        {
            TowerSelectedClicked = i;
            upgradeTowersNotSelectedBtn.SetActive(false);
            upgradeTowersBtn.SetActive(true);
            towerdetailsPanel.SetActive(true);
            backMainFromInventoryBtn.GetComponent<Button>().interactable = false;
            txtTowerDetails.text = "Tower Name : " + towersSelected[i].name+ "level = " + towersSelected[i].level;
            useTowerButton.SetActive(true);
            useTowerButton.GetComponent<Button>().interactable = false;
            UnlockTowerBtn.SetActive(false);
            if (towersSelected[i].level < 5)
            {

                upgradeTowersBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                upgradeTowersBtn.GetComponent<Button>().interactable = false;
            }
            
        }
    }
    public void OnChoosingCards(int i)
    {
        towerdetailsPanel.SetActive(true);
        backMainFromInventoryBtn.GetComponent<Button>().interactable = false;
        upgradeTowersNotSelectedBtn.SetActive(true);
        shake = false;
        upgradeTowersBtn.SetActive(false);
        txtTowerDetails.text = "Tower Name : " + towersNotSelected[i].name + "level = " + towersNotSelected[i].level;
        towerdetailsPanel.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = true;
        TowerNotSelectedClicked = i;
        if(towersNotSelected[i].locked==true)
        {
            UnlockTowerBtn.SetActive(true);
            useTowerButton.SetActive(false);
            upgradeTowersNotSelectedBtn.GetComponent<Button>().interactable = false;
            
        }
        else
        {
            UnlockTowerBtn.SetActive(false);
            useTowerButton.SetActive(true);
            if (towersNotSelected[i].level < 5)
            {

                upgradeTowersNotSelectedBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                upgradeTowersBtn.GetComponent<Button>().interactable = false;
            }
           
        }
     
    }
    public void SwitchTowers(int i, int j)
    {
        TowerScript tower = towersSelected[i];
        towersSelected[i] = towersNotSelected[j];
        towersNotSelected[j] = tower;
        GameManager.instance.setSelectedTower(towersSelected);
            }
    public void OnPlayerClick(int i)
    {

        playerClicked = i;
        PlayerdetailsPanel.SetActive(true);
        backMainFromcharacterBtn.interactable = false;
        for(int j=0;j<GameManager.instance.players.Length;j++)
        {
            if (GameManager.instance.players[playerClicked] == GameManager.instance.getPlayer())
            {
                UsePlayerButton.SetActive(true);
                UsePlayerButton.GetComponent<Button>().interactable = false;
                UnlockPlayerBtn.SetActive(false);
                if (GameManager.instance.players[playerClicked].level < 5)
                {
                    upgradePlayersBtn.GetComponent<Button>().interactable = true;
                }
                else { upgradePlayersBtn.GetComponent<Button>().interactable = false; }

            }
            else if (GameManager.instance.players[playerClicked].locked == true)
            {
                UsePlayerButton.SetActive(false);
                UnlockPlayerBtn.SetActive(true);
                upgradePlayersBtn.GetComponent<Button>().interactable = false;
            }
            else if(GameManager.instance.players[playerClicked].locked==false)
            {
                UsePlayerButton.GetComponent<Button>().interactable = true;
                UsePlayerButton.SetActive(true);
                UnlockPlayerBtn.SetActive(false);
                if (GameManager.instance.players[playerClicked].level < 5)
                {
                    upgradePlayersBtn.GetComponent<Button>().interactable = true;
                }else { upgradePlayersBtn.GetComponent<Button>().interactable = false; }
            }
            
        }
        txtPlayerDetails.text = "Character Name : " + GameManager.instance.players[playerClicked].name + " Magic1 description" + GameManager.instance.players[playerClicked].magic1.description + "level = " + GameManager.instance.players[playerClicked].level ;

      /*  if (GameManager.instance.players[k].name == GameManager.instance.data.SelectedPlayer)
        {
            UnlockPlayerBtn.SetActive(false);
            UsePlayerButton.SetActive(true);
            UsePlayerButton.GetComponent<Button>().interactable = false;
            return;
        }*/
       /*for (int i = 0; i < GameManager.instance.players.Length)
        {
            
           for(int j =0;GameManager)
            if(GameManager.instance.players[k].name==GameManager.instance.data.UnlockedPlayers[])
        }*/
        
        
    }
    public void OnGunClick(int i)
    {

        GunClicked = (byte)i;
        GunDetailsPanel.SetActive(true);
        backMainFromcharacterBtn.interactable = false;
        for (int j = 0; j < GameManager.instance.guns.Length; j++)
        {
            if (GameManager.instance.guns[GunClicked] == GameManager.instance.getGun())
            {
                UseGunButton.SetActive(true);
                UseGunButton.GetComponent<Button>().interactable = false;
                UnlockGunBtn.SetActive(false);
                if (GameManager.instance.guns[GunClicked].level < 5)
                {
                    upgradeGunBtn.GetComponent<Button>().interactable = true;
                }
                else { upgradeGunBtn.GetComponent<Button>().interactable = false; }

            }
            else if (GameManager.instance.guns[GunClicked].locked == true)
            {
                UseGunButton.SetActive(false);
                UnlockGunBtn.SetActive(true);
                upgradeGunBtn.GetComponent<Button>().interactable = false;
            }
            else if (GameManager.instance.guns[GunClicked].locked == false)
            {
                UseGunButton.GetComponent<Button>().interactable = true;
                UseGunButton.SetActive(true);
                UnlockGunBtn.SetActive(false);
                if (GameManager.instance.guns[GunClicked].level < 5)
                {
                    upgradeGunBtn.GetComponent<Button>().interactable = true;
                }
                else { upgradeGunBtn.GetComponent<Button>().interactable = false; }
            }

        }
        txtGunDetails.text = "Gun Name : " + GameManager.instance.guns[GunClicked].name + " Speed = " + GameManager.instance.guns[GunClicked].speed + "level = " + GameManager.instance.guns[GunClicked].level;
    }
    public void UseButton() {
        GameManager.instance.setPlayer(GameManager.instance.players[playerClicked]);
        PlayerdetailsPanel.SetActive(false);
        backMainFromcharacterBtn.interactable = true;
        //Debug.Log(GameManager.instance.getPlayer().name);


    }
    public void UseGun()
    {
        GameManager.instance.setGun(GameManager.instance.guns[GunClicked]);
        GunDetailsPanel.SetActive(false);
        backMainFromShopBtn.interactable = true;
        //Debug.Log(GameManager.instance.getPlayer().name);


    }
    public void UnlockPlayerButton()
    {
        GameManager.instance.players[playerClicked].locked = false;
        PlayerdetailsPanel.SetActive(false);
        backMainFromcharacterBtn.interactable = true;
    }
    public void UpgradeTowerBtn() {
        towersSelected[TowerSelectedClicked].level++;
        towerdetailsPanel.SetActive(false);
        backMainFromInventoryBtn.GetComponent<Button>().interactable = true;
    }
    public void UpgradePlayerBtn() {
        if (GameManager.instance.players[playerClicked].level <= 5) { GameManager.instance.players[playerClicked].level++; }
        backMainFromcharacterBtn.interactable = true;
    }
    public void UpgradeGunBtn()
    {
        if (GameManager.instance.guns[GunClicked].level <= 5) {
            GameManager.instance.guns[GunClicked].level++;
            GunDetailsPanel.SetActive(false);
        }
        else
        {
            Debug.Log("cannot upgrade more");
        }
        backMainFromShopBtn.interactable = true;
    }
    public void UpgradeTowerNotSelectedBtn() {
        towersNotSelected[TowerNotSelectedClicked].level++;
        towerdetailsPanel.SetActive(false);
        backMainFromInventoryBtn.GetComponent<Button>().interactable = true;
    }
    public void AreYouSure()
    {
        towerdetailsPanel.SetActive(false);
        AreYouSureTowersPanel.SetActive(true);
        UnlockDetails.text = "Are you sure to Unlock  " + towersNotSelected[TowerNotSelectedClicked].name + "that costs " + towersNotSelected[TowerNotSelectedClicked].UnlockPrice +"Diamonds ?";
    }
    public void returnFromAreYouSurePanel() {
        AreYouSureTowersPanel.SetActive(false);
        backMainFromInventoryBtn.GetComponent<Button>().interactable = true;
    }
    public void UnlockTowerNotSelectedBtn() {
        
            AreYouSureTowersPanel.SetActive(false);
            towersNotSelected[TowerNotSelectedClicked].locked = false;
            towerdetailsPanel.SetActive(false);
            backMainFromInventoryBtn.GetComponent<Button>().interactable = true;
        if (towersNotSelected[TowerNotSelectedClicked].UnlockPrice > GameManager.instance.diamond)
        {
            //you don't have enough diamonds to unlock this tower panel
            //NotEnoughDiamonds.SetActive(true);
        }
        else
        {
            //congrats you unlocked this tower and diamond minus the price of the tower
            //GameManager.instance.diamond -= (byte)towersNotSelected[TowerNotSelectedClicked].UnlockPrice;
           // Congrats.SetActive(true);
        }
    }
    public void UnlockGun()
    {

        AreYouSureTowersPanel.SetActive(false);
        GameManager.instance.guns[GunClicked].locked = false;
        GunDetailsPanel.SetActive(false);
       backMainFromShopBtn.GetComponent<Button>().interactable = true;
        if (GameManager.instance.guns[GunClicked].UnlockPrice > GameManager.instance.diamond)
        {
            //you don't have enough diamonds to unlock this gun panel
            //NotEnoughDiamonds.SetActive(true);
        }
        else
        {
            //congrats you unlocked this gun and diamond minus the price of the gun
            //GameManager.instance.diamond -= (byte)GameMAnager.instance.guns[GunsClicked].UnlockPrice;
            // Congrats.SetActive(true);
        }
    }
    public void back()
    {
        PlayerdetailsPanel.gameObject.SetActive(false);
        backMainFromcharacterBtn.interactable = true;
    }
    public void backTowerDetailsPanel()
    {
        towerdetailsPanel.SetActive(false);
        backMainFromInventoryBtn.GetComponent<Button>().interactable = true;
    }
    public void backGunDetailsPanel()
    {
        GunDetailsPanel.SetActive(false);
        backMainFromInventoryBtn.GetComponent<Button>().interactable = true;
    }
    public void playPvm()
    {
     
        mainPanel.SetActive(false);
        SelectGameDiffPanel.SetActive(true);
    }
    public void option()
    {
       
        optionPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void shop()
    {
        
        shopPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void exit()
    {
        SaveSystem.SavePlayer();
        Application.Quit();
        Debug.Log("Quit !!!");
    }
    public void character()
    {
     
        mainPanel.SetActive(false);
        characterPanel.SetActive(true);
    }
    public void inventorypanel()
    {
       
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(true);
       
    }
    public void backMainFromInventory()
    {
        
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }
    public void backMainFromShop()
    {
        
        mainPanel.SetActive(true);
        shopPanel.SetActive(false);
    }
    public void backMainFromcharacter()
    {
       
        mainPanel.SetActive(true);
        characterPanel.SetActive(false);
      

    }
    public void backMainFromOption()
    {
      
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);
    }
    public void playPvp()
    {
        //assign to button when created
        SceneManager.LoadScene("pvp");
    }
    public void easy()
    {
        //
        SceneManager.LoadScene("pvm");
    }
    public void medium()
    {
        //
        SceneManager.LoadScene("pvm");
    }
    public void hard()
    {
        //
        SceneManager.LoadScene("pvm");
    }
}

