﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuManager : MonoBehaviour
{
    private bool testTowerClick = false;
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

   
   
    [Header("PLAYERS Panel")]
    public GameObject PlayerDetailsPanel;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerHealth;
    public TextMeshProUGUI PlayerLevel;
    byte playerClicked;
    public GameObject ButtomBarButton1;
    public GameObject ButtomBarButton2;
    public GameObject ButtomBarButton3;
    public GameObject ButtomBar;
    public Image magic1Image;
    public Image magic2Image;
    public GameObject upgradePlayerButton;
    public GameObject UsePlayerButton;
    public GameObject UnlockPlayerButton;
    public TextMeshProUGUI playerUpgradeValue;
    public TextMeshProUGUI playerUnlockValue;





    [Header("GUNS Panel")]
    public GameObject GunDetailsPanel;
    public TextMeshProUGUI GunName;
    public TextMeshProUGUI GunDescription;
    public TextMeshProUGUI GunDamage;
    public TextMeshProUGUI GunHitSpeed;
    public Image GunImage;
    byte GunClicked;
    public GameObject upgradeGunButton;
    public GameObject UseGunButton;
    public GameObject UnlockGunButton;
    public GameObject GunStarlvl1;
    public GameObject GunStarlvl2;
    public GameObject GunStarlvl3;
    public TextMeshProUGUI gunUpgradeValue;
    public TextMeshProUGUI gunUnlockValue;

    [Header("TOWERS Panel")]
    public GameObject TowerdetailsPanel;
    byte TowerNotSelectedClicked;
    public TextMeshProUGUI TowerName;
    public TextMeshProUGUI TowerDescription;
    public TextMeshProUGUI TowerDamage;
    public TextMeshProUGUI TowerHealth;
    public TextMeshProUGUI TowerTarget;
    public TextMeshProUGUI TowerHitSpeed;
    public Image towerImage;
    public GameObject upgradeTowerButton;
    public GameObject UseTowerButton;
    public GameObject UnlockTowerButton;
    private TowerScript lastTowerClicked;
    public GameObject TowerStarlvl1;
    public GameObject TowerStarlvl2;
    public GameObject TowerStarlvl3;
    public TextMeshProUGUI towerUpgradeValue;
    public TextMeshProUGUI towerUnlockValue;

    [Header("Shop")]
    public GameObject pack;
    public GameObject packContent;

    [Header("Setting")]
    public GameObject musicButton;
    public GameObject soundEffectButton;
    public GameObject notificationButton;
    public GameObject vibrateButton;

    private void Start()
    {
        towersMenuInstantiate();
        fillTowersSprites();
        playerMenuInstantiate();
        ShopMenuInstantiate();
        shopPackInstantiate();
    }

    void shopPackInstantiate()
    {
        foreach (GemScript p in GameManager.instance.packs)
        {
            GameObject inst = Instantiate(pack, packContent.transform);
            inst.GetComponent<packInf>().packwrite(p.gemCount, p.image, p.price, p.onSalePercentage);
        }
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
    public void settingMusic(bool active)
    {
        if (active)
        {
            musicButton.transform.GetChild(0).gameObject.SetActive(true);
            musicButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {

            musicButton.transform.GetChild(0).gameObject.SetActive(false);
            musicButton.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void settingSoundEffect(bool active)
    {
        if (active)
        {
            soundEffectButton.transform.GetChild(0).gameObject.SetActive(true);
            soundEffectButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            soundEffectButton.transform.GetChild(0).gameObject.SetActive(false);
            soundEffectButton.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void settingNotification(bool active)
    {
        if (active)
        {
            notificationButton.transform.GetChild(0).gameObject.SetActive(true);
            notificationButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            notificationButton.transform.GetChild(0).gameObject.SetActive(false);
            notificationButton.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void settingVibrate(bool active)
    {
        if (active)
        {
            vibrateButton.transform.GetChild(0).gameObject.SetActive(true);
            vibrateButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            vibrateButton.transform.GetChild(0).gameObject.SetActive(false);
            vibrateButton.transform.GetChild(1).gameObject.SetActive(true);
        }
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
        tower();
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
        myButton.onClick.AddListener(() => { OnNotSelectedTowerClick(i); });
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
        myButton.onClick.AddListener(() => { OnPlayerClick(i); });

    }
    public void OnPlayerClick(int i)
    {
        ButtomBarButton1.SetActive(false);
        ButtomBarButton2.SetActive(false);
        ButtomBarButton3.SetActive(false);
        ButtomBar.SetActive(false);
        playerClicked = (byte)i;

        PlayerDetailsPanel.SetActive(true);
       
        PlayerHealth.text = GameManager.instance.players[playerClicked].Get_health_player().ToString();
        PlayerName.text = GameManager.instance.players[playerClicked].name.ToString();
        PlayerLevel.text = GameManager.instance.players[playerClicked].level.ToString();
        magic1Image.sprite = GameManager.instance.players[playerClicked].magic1.image;
        magic2Image.sprite = GameManager.instance.players[playerClicked].magic2.image;
        playerUnlockValue.text = GameManager.instance.players[playerClicked].UnlockPrice.ToString();

        if (GameManager.instance.players[playerClicked].locked)
        {
            UnlockPlayerButton.SetActive(true);
            upgradePlayerButton.SetActive(false);
            UsePlayerButton.SetActive(false);
        }
        else
        {
            UnlockPlayerButton.SetActive(false);
            upgradePlayerButton.SetActive(true);
            UsePlayerButton.SetActive(true);
            if (GameManager.instance.players[playerClicked] == GameManager.instance.getPlayer())
            {
                UsePlayerButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                UsePlayerButton.GetComponent<Button>().interactable = true ;
            }
            if (GameManager.instance.players[playerClicked].level == 3)
            {
                upgradePlayerButton.GetComponent<Button>().interactable = false;
                playerUpgradeValue.text = GameManager.instance.players[playerClicked].UpgradePrice[1].ToString();
            }
            else
            {
                upgradePlayerButton.GetComponent<Button>().interactable = true;
                playerUpgradeValue.text = GameManager.instance.players[playerClicked].UpgradePrice[GameManager.instance.players[playerClicked].level - 1].ToString();
            }

        }



       
    }
    public void exitPlayerDetails()
    {
        PlayerDetailsPanel.SetActive(false);
        ButtomBarButton1.SetActive(true);
        ButtomBarButton2.SetActive(true);
        ButtomBarButton3.SetActive(true);
        ButtomBar.SetActive(true);
    }
    public void nextPlayer()
    {
        if (playerClicked != GameManager.instance.players.Length - 1)
        {
            OnPlayerClick(playerClicked + 1);
        }
        else
        {
            OnPlayerClick(0);
        }
    }
    public void PreviousPlayer()
    {
        if (playerClicked != 0)
        {
            OnPlayerClick(playerClicked -1 );
        }
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
    public void OnUsePlayer()
    {
        GameManager.instance.setPlayer(GameManager.instance.players[playerClicked]);
        UsePlayerButton.GetComponent<Button>().interactable = false;
    }
    public void OnUnlockPlayer()
    {
        GameManager.instance.players[playerClicked].locked = false;
        UnlockPlayerButton.SetActive(false);
        upgradePlayerButton.SetActive(true);
        UsePlayerButton.SetActive(true);
        upgradePlayerButton
.GetComponent<Button>().interactable = true;
        UsePlayerButton.GetComponent<Button>().interactable = true;
    }
    public void OnUpgradePlayer()
    {
        GameManager.instance.players[playerClicked].level++;
        PlayerHealth.text = GameManager.instance.players[playerClicked].Get_health_player().ToString();
        PlayerLevel.text = GameManager.instance.players[playerClicked].level.ToString();
       
        switch (GameManager.instance.players[playerClicked].level)
        {
            case 2:
                upgradePlayerButton.GetComponent<Button>().interactable = true;
                playerUpgradeValue.text = GameManager.instance.players[playerClicked].UpgradePrice[GameManager.instance.players[playerClicked].level - 1].ToString();
                break;
            case 3:
                upgradePlayerButton.GetComponent<Button>().interactable = false;
                break;
        }

    }
    public void RegisterListenerShop(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => { OnGunClick(i); });

    }
    public void OnGunClick(int i)
    {

        GunClicked = (byte)i;
        GunDetailsPanel.SetActive(true);
        GunName.text = GameManager.instance.guns[i].name;
        GunDamage.text = GameManager.instance.guns[i].Get_damage_Gun_player().ToString();
        GunHitSpeed.text = GameManager.instance.guns[i].Get_fireRate_Gun_player().ToString();
        GunDescription.text = GameManager.instance.guns[i].description;
        gunUnlockValue.text = GameManager.instance.guns[i].UnlockPrice.ToString();
        
        GunImage.sprite = GameManager.instance.guns[i].image;
       
        if (GameManager.instance.guns[i].locked)
        {
            UseGunButton.SetActive(false);
            upgradeGunButton.SetActive(false);
            UnlockGunButton.SetActive(true);
            GunStarlvl1.SetActive(false);
            GunStarlvl2.SetActive(false);
            GunStarlvl3.SetActive(false);
        }
        else
        {
            UseGunButton.SetActive(true);
            upgradeGunButton.SetActive(true);
            UnlockGunButton.SetActive(false);
            if (GameManager.instance.guns[i] == GameManager.instance.getGun())
            {
                UseGunButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                UseGunButton.GetComponent<Button>().interactable = true;
            }
            switch (GameManager.instance.guns[i].level)
            {
                case 1:
                    upgradeGunButton.GetComponent<Button>().interactable = true;
                    GunStarlvl1.SetActive(true);
                    GunStarlvl2.SetActive(false);
                    GunStarlvl3.SetActive(false);
                    gunUpgradeValue.text = GameManager.instance.guns[i].UpgradePrice[GameManager.instance.guns[i].level - 1].ToString();
                    break;
                case 2:
                    upgradeGunButton.GetComponent<Button>().interactable = true;
                    GunStarlvl1.SetActive(false);
                    GunStarlvl2.SetActive(true);
                    GunStarlvl3.SetActive(false);
                    gunUpgradeValue.text = GameManager.instance.guns[i].UpgradePrice[GameManager.instance.guns[i].level - 1].ToString();
                    break;
                case 3:
                    upgradeGunButton.GetComponent<Button>().interactable = false;
                    GunStarlvl1.SetActive(false);
                    GunStarlvl2.SetActive(false);
                    GunStarlvl3.SetActive(true);
                    gunUpgradeValue.text = GameManager.instance.guns[i].UpgradePrice[1].ToString();
                    break;
            }
        }
    }
    public void exitGunDetailsPanel()
    {
        GunDetailsPanel.SetActive(false);
    }
    public void OnNotSelectedTowerClick(int i)
    {
        lastTowerClicked = GameManager.instance.GetNonSelectedTowers()[i];
        TowerNotSelectedClicked = (byte)i;
        TowerdetailsPanel.SetActive(true);
        TowerDescription.text = GameManager.instance.GetNonSelectedTowers()[i].description;
        TowerName.text = GameManager.instance.GetNonSelectedTowers()[i].name;
        TowerDamage.text = GameManager.instance.GetNonSelectedTowers()[i].Get_damage_player().ToString();
        TowerHealth.text = GameManager.instance.GetNonSelectedTowers()[i].Get_health_player().ToString();
        TowerHitSpeed.text = GameManager.instance.GetNonSelectedTowers()[i].Get_fireRate_player().ToString();
        TowerTarget.text = GameManager.instance.GetNonSelectedTowers()[i].target;
        towerImage.sprite = GameManager.instance.GetNonSelectedTowers()[i].image;
        towerUnlockValue.text = GameManager.instance.GetNonSelectedTowers()[i].UnlockPrice.ToString();

        if (GameManager.instance.GetNonSelectedTowers()[i].locked)
        {
            UnlockTowerButton.SetActive(true);
            UseTowerButton.SetActive(false);
            upgradeTowerButton.SetActive(false);
            TowerStarlvl1.SetActive(false);
            TowerStarlvl2.SetActive(false);
            TowerStarlvl3.SetActive(false);
        }
        else
        {
            UnlockTowerButton.SetActive(false);
            UseTowerButton.SetActive(true);
            upgradeTowerButton.SetActive(true);
            UseTowerButton.GetComponent<Button>().interactable =true;
            switch(GameManager.instance.GetNonSelectedTowers()[i].level)
            {
                case 1:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
                    TowerStarlvl1.SetActive(true);
                    TowerStarlvl2.SetActive(false);
                    TowerStarlvl3.SetActive(false);
                    towerUpgradeValue.text = GameManager.instance.GetNonSelectedTowers()[i].UpgradePrice[GameManager.instance.GetNonSelectedTowers()[i].level - 1].ToString();
                    break;
                case 2:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
                    TowerStarlvl1.SetActive(false);
                    TowerStarlvl2.SetActive(true);
                    TowerStarlvl3.SetActive(false);
                    towerUpgradeValue.text = GameManager.instance.GetNonSelectedTowers()[i].UpgradePrice[GameManager.instance.GetNonSelectedTowers()[i].level - 1].ToString();
                    break;
                case 3:
                    upgradeTowerButton.GetComponent<Button>().interactable = false;
                    TowerStarlvl1.SetActive(false);
                    TowerStarlvl2.SetActive(false);
                    TowerStarlvl3.SetActive(true);
                    towerUpgradeValue.text = GameManager.instance.GetNonSelectedTowers()[i].UpgradePrice[1].ToString();
                    break;
            }
         
               
           
        }



    }
    public void OnSelectedTowerClick(int i)
    {
      
        if (testTowerClick == true)
        {
            SwitchTowers(i, TowerNotSelectedClicked);
            fillTowersSprites();
            testTowerClick = false;
        }
        else
        {
            lastTowerClicked = GameManager.instance.GetSelectedTowers()[i];
            TowerdetailsPanel.SetActive(true);
            TowerDescription.text = GameManager.instance.GetSelectedTowers()[i].description;
            TowerName.text = GameManager.instance.GetSelectedTowers()[i].name;
            TowerDamage.text = GameManager.instance.GetSelectedTowers()[i].Get_damage_player().ToString();
            TowerHealth.text = GameManager.instance.GetSelectedTowers()[i].Get_health_player().ToString();
            TowerHitSpeed.text = GameManager.instance.GetSelectedTowers()[i].Get_fireRate_player().ToString();
            TowerTarget.text = GameManager.instance.GetSelectedTowers()[i].target;
            towerImage.sprite = GameManager.instance.GetSelectedTowers()[i].image;
            
            UnlockTowerButton.SetActive(false);
            UseTowerButton.SetActive(true);
            upgradeTowerButton.SetActive(true);
            UseTowerButton.GetComponent<Button>().interactable = false;
            switch (GameManager.instance.GetSelectedTowers()[i].level)
            {
                case 1:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
                    TowerStarlvl1.SetActive(true);
                    TowerStarlvl2.SetActive(false);
                    TowerStarlvl3.SetActive(false);
                    towerUpgradeValue.text = GameManager.instance.GetSelectedTowers()[i].UpgradePrice[GameManager.instance.GetSelectedTowers()[i].level - 1].ToString();
                    break;
                case 2:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
                    TowerStarlvl1.SetActive(false);
                    TowerStarlvl2.SetActive(true);
                    TowerStarlvl3.SetActive(false);
                    towerUpgradeValue.text = GameManager.instance.GetSelectedTowers()[i].UpgradePrice[GameManager.instance.GetSelectedTowers()[i].level - 1].ToString();
                    break;
                case 3:
                    upgradeTowerButton.GetComponent<Button>().interactable = false;
                    TowerStarlvl1.SetActive(false);
                    TowerStarlvl2.SetActive(false);
                    TowerStarlvl3.SetActive(true);
                    towerUpgradeValue.text = GameManager.instance.GetSelectedTowers()[i].UpgradePrice[1].ToString();
                    break;
            }
        }


    }
    public void exitTowerDetailsPanel()
    {
        TowerdetailsPanel.SetActive(false);
    }
    private void SwitchTowers(int i, int j)
    {
        TowerScript tower =GameManager.instance.GetSelectedTowers()[i];
        GameManager.instance.GetSelectedTowers()[i] = GameManager.instance.GetNonSelectedTowers()[j];
        GameManager.instance.GetNonSelectedTowers()[j] = tower;
        GameManager.instance.setSelectedTower(GameManager.instance.GetSelectedTowers());
    }
    public void OnUseTowerClick()
    {
        TowerdetailsPanel.SetActive(false);
        testTowerClick = true;
        
    }
    public void OnUseGunClick()
    {
        GameManager.instance.setGun(GameManager.instance.guns[GunClicked]);
        UseGunButton.GetComponent<Button>().interactable = false;
    }
    public void OnUpgradeTowerClick()
    {
        lastTowerClicked.level++;
        TowerDamage.text = lastTowerClicked.Get_damage_player().ToString();
        TowerHealth.text = lastTowerClicked.Get_health_player().ToString();
        TowerHitSpeed.text = lastTowerClicked.Get_fireRate_player().ToString();
       
        switch (lastTowerClicked.level)
        {
            case 2:
                upgradeTowerButton.GetComponent<Button>().interactable = true;
                towerUpgradeValue.text = lastTowerClicked.UpgradePrice[lastTowerClicked.level - 1].ToString();
                TowerStarlvl1.SetActive(false);
                TowerStarlvl2.SetActive(true);
                TowerStarlvl3.SetActive(false);
                break;
            case 3:
                upgradeTowerButton.GetComponent<Button>().interactable = false;
                TowerStarlvl1.SetActive(false);
                TowerStarlvl2.SetActive(false);
                TowerStarlvl3.SetActive(true);
                break;
        }
    }
    public void OnUpgradeGunClick()
    {
        GameManager.instance.guns[GunClicked].level++;
        GunDamage.text = GameManager.instance.guns[GunClicked].Get_damage_Gun_player().ToString();
        GunHitSpeed.text = GameManager.instance.guns[GunClicked].Get_fireRate_Gun_player().ToString();
        
        switch (GameManager.instance.guns[GunClicked].level)
        {
            case 2:
                upgradeGunButton.GetComponent<Button>().interactable = true;
                gunUpgradeValue.text = GameManager.instance.guns[GunClicked].UpgradePrice[GameManager.instance.guns[GunClicked].level - 1].ToString();
                GunStarlvl1.SetActive(false);
                GunStarlvl2.SetActive(true);
                GunStarlvl3.SetActive(false);
                break;
            case 3:
                upgradeGunButton.GetComponent<Button>().interactable = false;
                GunStarlvl1.SetActive(false);
                GunStarlvl2.SetActive(false);
                GunStarlvl3.SetActive(true);
                break;
        }
    }
    public void OnUnlockTowerClick()
    {
        lastTowerClicked.locked = false;
        UnlockTowerButton.SetActive(false);
        UseTowerButton.SetActive(true);
        upgradeTowerButton.SetActive(true);
        UseTowerButton.GetComponent<Button>().interactable = true;
        upgradeTowerButton.GetComponent<Button>().interactable = true;
        TowerStarlvl1.SetActive(true);
        TowerStarlvl2.SetActive(false);
        TowerStarlvl3.SetActive(false);
        towerUpgradeValue.text = lastTowerClicked.UpgradePrice[0].ToString();


    }
    public void OnUnlockGunClick() {
        GameManager.instance.guns[GunClicked].locked = false;
        UnlockGunButton.SetActive(false);
        UseGunButton.SetActive(true);
        upgradeGunButton.SetActive(true);
        UseGunButton.GetComponent<Button>().interactable = true;
        upgradeGunButton.GetComponent<Button>().interactable = true;
        GunStarlvl1.SetActive(true);
        GunStarlvl2.SetActive(false);
        GunStarlvl3.SetActive(false);
        gunUpgradeValue.text = GameManager.instance.guns[GunClicked].UpgradePrice[0].ToString();
    }
}

