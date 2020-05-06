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
        GunDetailsPanel.SetActive(true);
        GunName.text = GameManager.instance.guns[i].name;
        GunDamage.text = GameManager.instance.guns[i].Get_damage_Gun_player().ToString();
        GunHitSpeed.text = GameManager.instance.guns[i].Get_fireRate_Gun_player().ToString();
        GunDescription.text = GameManager.instance.guns[i].description;
        GunImage.sprite = GameManager.instance.guns[i].image;
        GunClicked = (byte)i;
        if (GameManager.instance.guns[i].locked)
        {
            UseGunButton.SetActive(false);
            upgradeGunButton.SetActive(false);
            UnlockGunButton.SetActive(true);
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
            if (GameManager.instance.guns[i].level >= 3)
            {
                upgradeGunButton.GetComponent<Button>().interactable = false;
            }
            else
                upgradeGunButton.GetComponent<Button>().interactable = true;
        }
    }
    public void exitGunDetailsPanel()
    {
        GunDetailsPanel.SetActive(false);
    }
    public void OnNotSelectedTowerClick(int i)
    {
        TowerNotSelectedClicked = (byte)i;
        TowerdetailsPanel.SetActive(true);
        TowerDescription.text = GameManager.instance.GetNonSelectedTowers()[i].description;
        TowerName.text = GameManager.instance.GetNonSelectedTowers()[i].name;
        TowerDamage.text = GameManager.instance.GetNonSelectedTowers()[i].Get_damage_player().ToString();
        TowerHealth.text = GameManager.instance.GetNonSelectedTowers()[i].Get_health_player().ToString();
        TowerHitSpeed.text = GameManager.instance.GetNonSelectedTowers()[i].Get_fireRate_player().ToString();
        TowerTarget.text = GameManager.instance.GetNonSelectedTowers()[i].target;
        towerImage.sprite = GameManager.instance.GetNonSelectedTowers()[i].image;
        if (GameManager.instance.GetNonSelectedTowers()[i].locked)
        {
            UnlockTowerButton.SetActive(true);
            UseTowerButton.SetActive(false);
            upgradeTowerButton.SetActive(false);
        }
        else
        {
            UnlockTowerButton.SetActive(false);
            UseTowerButton.SetActive(true);
            upgradeTowerButton.SetActive(true);
            UseTowerButton.GetComponent<Button>().interactable =true;
            if (GameManager.instance.GetNonSelectedTowers()[i].level >= 3)
            {
                upgradeTowerButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                upgradeTowerButton.GetComponent<Button>().interactable = true;
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
            if (GameManager.instance.GetSelectedTowers()[i].level >= 3)
            {
                upgradeTowerButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                upgradeTowerButton.GetComponent<Button>().interactable = true;
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
    public void OnUpgradeTowerClick() { }
    public void OnUpgradeGunClick() { }
    public void OnUnlockTowerClick() { }
    public void OnUnlockGunClick() { }
}

