using System.Collections;
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



    [Header("GUNS Shop")]
    public GameObject GunDetailsPanel;
    public TMPro.TextMeshProUGUI GunName;
    public TMPro.TextMeshProUGUI GunDescription;
    public TMPro.TextMeshProUGUI GunDamage;
    public TMPro.TextMeshProUGUI GunHitSpeed;
    public Image GunImage;
    byte GunClicked;
    public GameObject upgradeGunButton;
    public GameObject UseGunButton;
    public GameObject UnlockGunButton;
    public GameObject GunStarlvl1;
    public GameObject GunStarlvl2;
    public GameObject GunStarlvl3;
    public TMPro.TextMeshProUGUI gunUpgradeValue;
    public TMPro.TextMeshProUGUI gunUnlockValue;

    [Header("TOWERS Shop")]
    public GameObject TowerdetailsPanel;
    byte TowerNotSelectedClicked;
    public TMPro.TextMeshProUGUI TowerName;
    public TMPro.TextMeshProUGUI TowerDescription;
    public TMPro.TextMeshProUGUI TowerDamage;
    public TMPro.TextMeshProUGUI TowerHealth;
    public TMPro.TextMeshProUGUI TowerTarget;
    public TMPro.TextMeshProUGUI TowerHitSpeed;
    public Image towerImage;
    public GameObject upgradeTowerButton;
    public GameObject UseTowerButton;
    public GameObject UnlockTowerButton;
    private TowerScript lastTowerClicked;
    public GameObject TowerStarlvl1;
    public GameObject TowerStarlvl2;
    public GameObject TowerStarlvl3;
    public TMPro.TextMeshProUGUI towerUpgradeValue;
    public TMPro.TextMeshProUGUI towerUnlockValue;





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
        gunUpgradeValue.text = GameManager.instance.guns[i].UpgradePrice[GameManager.instance.guns[i].level - 1].ToString();
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
                    break;
                case 2:
                    upgradeGunButton.GetComponent<Button>().interactable = true;
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
        towerUpgradeValue.text = GameManager.instance.GetNonSelectedTowers()[i].UpgradePrice[GameManager.instance.GetNonSelectedTowers()[i].level - 1].ToString();
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
                    break;
                case 2:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
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
            towerUpgradeValue.text = GameManager.instance.GetSelectedTowers()[i].UpgradePrice[GameManager.instance.GetSelectedTowers()[i].level - 1].ToString();
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
                    break;
                case 2:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
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
        GunDetailsPanel.SetActive(false);
    }
    public void OnUpgradeTowerClick()
    {
        lastTowerClicked.level++;
        TowerDamage.text = lastTowerClicked.Get_damage_player().ToString();
        TowerHealth.text = lastTowerClicked.Get_health_player().ToString();
        TowerHitSpeed.text = lastTowerClicked.Get_fireRate_player().ToString();
        towerUpgradeValue.text = lastTowerClicked.UpgradePrice[lastTowerClicked.level - 1].ToString();
        switch (lastTowerClicked.level)
        {
            case 2:
                upgradeTowerButton.GetComponent<Button>().interactable = true;
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
        gunUpgradeValue.text = GameManager.instance.guns[GunClicked].UpgradePrice[GameManager.instance.guns[GunClicked].level - 1].ToString();
        switch (GameManager.instance.guns[GunClicked].level)
        {
            case 2:
                upgradeGunButton.GetComponent<Button>().interactable = true;
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

