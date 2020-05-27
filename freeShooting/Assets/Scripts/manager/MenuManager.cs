using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuManager : MonoBehaviour
{
    public float amount;
    public float speed;
    
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
    public TextMeshProUGUI gemText;

    short lastCardSelected;
    bool stopUseAnimation = false;

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
    public GameObject Hero3d;

    [Header("Unlock and upgrade Panel")]
    public GameObject CongratulationPanel;
    public Image unlockedObjectSprite;
    public TextMeshProUGUI unlockedObjectDescription;
 




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
    public GameObject towerDamagePanel;
    public GameObject towerFirePointPanel;
    public GameObject towerTargetPanel;
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
    public GameObject confirmPanel;
    public TextMeshProUGUI ConfirmTxt;
    public byte packClicked;


    [Header("Setting")]
    public GameObject musicButton;
    public GameObject soundEffectButton;
    public GameObject notificationButton;
    public GameObject vibrateButton;

    [Header("Magics")]
    public GameObject MagicDetailsPanel;
    public TextMeshProUGUI MagicName;
    public TextMeshProUGUI MagicDescription;
    public Image MagicImage;

    [Header("Hero")]
    public GameObject HeroMain;
    [Header("Profile")]
    public GameObject profilePanel;
    public GameObject changeNamePanel;
    public GameObject ChangeNameField;
    public GameObject Erreur;
    public TextMeshProUGUI WelcomTxt;
    public RawImage playerPicture;
    public GameObject pictureButton;
    public GameObject ChangePicturePanel;
    public GameObject ContentPanel;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI gamePlayed;
    public TextMeshProUGUI win;
    public TextMeshProUGUI lose;



    private void Start()
    {
        towersMenuInstantiate();
        fillTowersSprites();
        playerMenuInstantiate();
        ShopMenuInstantiate();
        shopPackInstantiate();
        LevelSystem.instance.ADDxp();
        gemText.text = GameManager.instance.diamond.ToString();
        
        if (!GameManager.instance.FirstTime)
        {
            changeHeroMain();

        }
        else
        {
            changeNamePanel.SetActive(true);
        }
    }
   
    public void picturesInstantiate()
    {
        for (int i = 0; i < GameManager.instance.Pictures.Length; i++)
        {
            ChangePicturePanel.SetActive(true);
            GameObject go = Instantiate(pictureButton, ContentPanel.transform);
            go.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.Pictures[i];
            RegisterListenerPicture(go, i);
        }
    }

    public void RegisterListenerPicture(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => { OnPictureChoose(i); });
    }
    public void OnPictureChoose(int i)
    {
        GameManager.instance.playerPicture = (byte)i;
        SaveSystem.SavePlayer();
        playerPicture.texture = GameManager.instance.Pictures[GameManager.instance.playerPicture].texture;
        exitChangePicture();
        if (GameManager.instance.FirstTime)
        {
            changeHeroMain();
            GameManager.instance.FirstTime = false;
        }

    }
    public void exitChangePicture()
    {
        ChangePicturePanel.SetActive(false);
        if (GameManager.instance.FirstTime)
        {
            changeHeroMain();
            GameManager.instance.FirstTime = false;
        }

    }

    public void OnclickEditName()
    {
        changeNamePanel.SetActive(true);
      //  ChangeNameField.GetComponent<TextMeshProUGUI>().text = "";

    }
    public void exitChangeName()
    {
        changeNamePanel.SetActive(false);
        if (GameManager.instance.FirstTime)
        {
            picturesInstantiate();
        }
    }
    IEnumerator HideErreur()
    {
        Erreur.SetActive(true);
        yield return new WaitForSeconds(3);
        Erreur.SetActive(false);
    }
    public void ValidName()
    {
        if (ChangeNameField.GetComponent<TextMeshProUGUI>().text.Length <= 4)
        {
            StartCoroutine(HideErreur());
        }
        else
        {
            Erreur.SetActive(false);
            GameManager.instance.playerName = ChangeNameField.GetComponent<TextMeshProUGUI>().text;
            SaveSystem.SavePlayer();
            WelcomTxt.text = "Welcom " + GameManager.instance.playerName;
            changeNamePanel.SetActive(false);
            if (GameManager.instance.FirstTime)
            {
                picturesInstantiate();
            }
        }
    }
    public void onProfileClick()
    {
        damage.text = GameManager.instance.damageDone.ToString();
        win.text = GameManager.instance.winCount.ToString();
        lose.text = GameManager.instance.loseCount.ToString();
        gamePlayed.text = GameManager.instance.gamePlayed.ToString();
        playerPicture.texture = GameManager.instance.Pictures[GameManager.instance.playerPicture].texture;
        WelcomTxt.text = "Welcom " + GameManager.instance.playerName;
        HeroMain.SetActive(false);
        profilePanel.SetActive(true);
    }
    public void exitProfilePanel()
    {
        profilePanel.SetActive(false);
        changeHeroMain();
    }
    public void changeHeroMain()
    {
        HeroMain.SetActive(true);
        for (short i = 0; i < 4; i++)
        {
            if (GameManager.instance.players[i] == GameManager.instance.getPlayer())
            {
                HeroMain.transform.GetChild(i).gameObject.SetActive(true);
                HeroMain.transform.GetChild(i).GetComponent<Animator>().SetFloat("x", 0.5f);
                /*for (int j = i++; j < 4; j++)
                {
                    HeroMain.transform.GetChild(j).gameObject.SetActive(false);
                }
                break;*/
            }
            else
            {
                HeroMain.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }

    void shopPackInstantiate()
    {
        byte i= 0;
        foreach (GemScript p in GameManager.instance.packs)
        {
            GameObject inst = Instantiate(pack, packContent.transform);
            inst.GetComponent<packInf>().packwrite(p.gemCount, p.image, p.price, p.onSalePercentage);
            RegisterListenerShopPack(inst, i);
            i++;
        }
    }
    public void RegisterListenerShopPack(GameObject obj, int i)
    {
        obj.transform.GetComponentInChildren<Button>().onClick.AddListener(() => { OnPackClick(i); });
    }
    public void OnPackClick(int i){
        packClicked = (byte)i;
        confirmPanel.SetActive(true);
        ConfirmTxt.text="Would you like to buy pack of "+ GameManager.instance.packs[i].gemCount + " Gem for "+ GameManager.instance.packs[i].price+ "$ ?";
        


    }
    public void OnConfirmBuying()
    {
        confirmPanel.SetActive(false);
        StartCoroutine(CoinAnimationAdd((ushort)GameManager.instance.packs[packClicked].gemCount));
        CongratulationPanel.SetActive(true);
        unlockedObjectSprite.sprite = GameManager.instance.packs[packClicked].image;
        unlockedObjectDescription.text = "You bought " + GameManager.instance.packs[packClicked].gemCount + " gem . <br> Now you can use it to upgrade or to unlock";
    }
    public void exitConfirmPanel()
    {
        confirmPanel.SetActive(false);    }
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
        stopUseAnimation = true;
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
        stopUseAnimation = true;
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
        HeroMain.SetActive(false);
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
        changeHeroMain();
        settingPanel.SetActive(false);
    }
    public void shop()
    {
        stopUseAnimation = true;
        HeroMain.SetActive(false);
        shopPanel.SetActive(true);
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }
    public void main()
    {
        stopUseAnimation = true;
        shopPanel.SetActive(false);
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        changeHeroMain();
    }
    public void inventory()
    {
        HeroMain.SetActive(false);
        shopPanel.SetActive(false);
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        tower();
    }
    public void fillTowersSprites()
    {
        for (int i = 0; i < 6; i++)
        {
            SelectedTowersPanel.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.GetSelectedTowers()[i].image;
        }
        for (int j = 0; j < GameManager.instance.GetNonSelectedTowers().Length; j++)
        {
            if (GameManager.instance.GetNonSelectedTowers()[j].locked == false)
            {
                NotSelectedTowersPanel.transform.GetChild(j).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.GetNonSelectedTowers()[j].image;
            }
            else
            {
                NotSelectedTowersPanel.transform.GetChild(j).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.GetNonSelectedTowers()[j].Lockedimage;
            }
           
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
           
            
            RegisterListener(go, i);
        }
        fillPlayersprites();


    }
    private void fillPlayersprites()
    {
        for (int i = 0; i < GameManager.instance.players.Length; i++)
        {
            if (GameManager.instance.players[i].locked == false)
            {
                HerosPanel.transform.GetChild(i).GetComponentInChildren<Image>().sprite = GameManager.instance.players[i].image;
            }
            else
            {
                HerosPanel.transform.GetChild(i).GetComponentInChildren<Image>().sprite = GameManager.instance.players[i].Lockedimage;
            }
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
        Hero3d.SetActive(true);
        for (int j = 0; j < 4; j++)
        {
            Hero3d.transform.GetChild(j).gameObject.SetActive(false);
        }
        Hero3d.transform.GetChild(i).gameObject.SetActive(true);
        Hero3d.transform.GetChild(i).GetComponent<Animator>().SetFloat("x", 0.5f);

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
            if(GameManager.instance.diamond< GameManager.instance.players[playerClicked].UnlockPrice)
            {
                UnlockPlayerButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                UnlockPlayerButton.GetComponent<Button>().interactable = true;
            }
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
                if (GameManager.instance.diamond < GameManager.instance.players[i].UpgradePrice[GameManager.instance.players[i].level - 1])
                {
                    upgradePlayerButton.GetComponent<Button>().interactable = false;
                }
            }

          

        } 
    }
    public void exitPlayerDetails()
    {
        Hero3d.SetActive(false);
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
           
          
            RegisterListenerShop(go, i);
        }
        FillGunsSprite();
    }
    private void FillGunsSprite()
    {
        for (int i = 0; i < GameManager.instance.guns.Length; i++)
        {
           
            if (GameManager.instance.guns[i].locked == false)
            {
                GunsPanel.transform.GetChild(i).GetComponentInChildren<Image>().sprite = GameManager.instance.guns[i].image;
            }
            else
            {
                GunsPanel.transform.GetChild(i).GetComponentInChildren<Image>().sprite = GameManager.instance.guns[i].Lockedimage;
            }

           
        }
    }
    public void OnUsePlayer()
    {
        GameManager.instance.setPlayer(GameManager.instance.players[playerClicked]);
        UsePlayerButton.GetComponent<Button>().interactable = false;
        SaveSystem.SavePlayer();
    }

    bool hero = false;
    public void OnUnlockPlayer()
    {
        Hero3d.SetActive(false);
        hero = true;
        GameManager.instance.players[playerClicked].locked = false;
        UnlockPlayerButton.SetActive(false);
        upgradePlayerButton.SetActive(true);
        UsePlayerButton.SetActive(true);
        upgradePlayerButton.GetComponent<Button>().interactable = true;
        UsePlayerButton.GetComponent<Button>().interactable = true;
        // GameManager.instance.diamond -= GameManager.instance.players[playerClicked].UnlockPrice;
        // gemText.text = GameManager.instance.diamond.ToString();
        StartCoroutine(CoinAnimation(GameManager.instance.players[playerClicked].UnlockPrice));
        UnlockObject(GameManager.instance.players[playerClicked].name, GameManager.instance.players[playerClicked].image);
        fillPlayersprites();
        //exitPlayerDetails();
        SaveSystem.SavePlayer();


    }
    private void UnlockObject(string name,Sprite image)
    {

        CongratulationPanel.SetActive(true);
        unlockedObjectSprite.sprite = image;
        unlockedObjectDescription.text = "You Unlocked the " + name + ".<br> You can select it or use it now.";

    }
    private void UpgradeObject(string name,Sprite image ,byte lvl)
    {
        CongratulationPanel.SetActive(true);
        unlockedObjectSprite.sprite = image;
        unlockedObjectDescription.text = "You Upgraded the " + name + " to level "+lvl+".";
    }
    public void OnUpgradePlayer()
    {
        Hero3d.SetActive(false);
        hero = true;
        //exitPlayerDetails();
        StartCoroutine(CoinAnimation(GameManager.instance.players[playerClicked].UpgradePrice[GameManager.instance.players[playerClicked].level - 1]));
        GameManager.instance.players[playerClicked].level++;
        PlayerHealth.text = GameManager.instance.players[playerClicked].Get_health_player().ToString();
        PlayerLevel.text = GameManager.instance.players[playerClicked].level.ToString();
        //gemText.text = GameManager.instance.diamond.ToString();
        switch (GameManager.instance.players[playerClicked].level)
        {
            case 2:
                upgradePlayerButton.GetComponent<Button>().interactable = true;
                playerUpgradeValue.text = GameManager.instance.players[playerClicked].UpgradePrice[GameManager.instance.players[playerClicked].level - 1].ToString();
                if (GameManager.instance.diamond < GameManager.instance.players[playerClicked].UpgradePrice[GameManager.instance.players[playerClicked].level - 1])
                {
                    upgradePlayerButton.GetComponent<Button>().interactable = false;
                }
                break;
            case 3:
                upgradePlayerButton.GetComponent<Button>().interactable = false;
                break;
        }
        UpgradeObject(GameManager.instance.players[playerClicked].name, GameManager.instance.players[playerClicked].image, GameManager.instance.players[playerClicked].level );

        SaveSystem.SavePlayer();

    }
    public void OnMagic1Click()
    {
        Hero3d.SetActive(false);
        MagicDetailsPanel.SetActive(true);
        MagicImage.sprite = GameManager.instance.players[playerClicked].magic1.image;
        MagicName.text = GameManager.instance.players[playerClicked].magic1.name;
        MagicDescription.text = GameManager.instance.players[playerClicked].magic1.description;
       
    }
    public void OnMagic2Click()
    {
        Hero3d.SetActive(false);
        MagicDetailsPanel.SetActive(true);
        MagicImage.sprite = GameManager.instance.players[playerClicked].magic2.image;
        MagicName.text = GameManager.instance.players[playerClicked].magic2.name;
        MagicDescription.text = GameManager.instance.players[playerClicked].magic2.description;
    }
    public void exitMagicDetailsPanel()
    {
        Hero3d.SetActive(true);
        for (int j = 0; j < 4; j++)
        {
            Hero3d.transform.GetChild(j).GetComponent<Animator>().SetFloat("x", 0.5f);
        }
        MagicDetailsPanel.SetActive(false);
    }
    public void exitCongratulationPanel()
    {
        if (hero)
        {
            Hero3d.SetActive(true);
            hero = false;
            for (int j = 0; j < 4; j++)
            {
                Hero3d.transform.GetChild(j).GetComponent<Animator>().SetFloat("x", 0.5f);
            }
        }
        CongratulationPanel.SetActive(false);
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
            if (GameManager.instance.diamond < GameManager.instance.guns[GunClicked].UnlockPrice)
            {
                UnlockGunButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                UnlockGunButton.GetComponent<Button>().interactable = true;
            }
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
                    if (GameManager.instance.diamond < GameManager.instance.guns[i].UpgradePrice[GameManager.instance.guns[i].level - 1])
                    {
                        upgradeGunButton.GetComponent<Button>().interactable = false;
                    }
                    break;
                case 2:
                    upgradeGunButton.GetComponent<Button>().interactable = true;
                    GunStarlvl1.SetActive(false);
                    GunStarlvl2.SetActive(true);
                    GunStarlvl3.SetActive(false);
                    gunUpgradeValue.text = GameManager.instance.guns[i].UpgradePrice[GameManager.instance.guns[i].level - 1].ToString();
                    if (GameManager.instance.diamond < GameManager.instance.guns[i].UpgradePrice[GameManager.instance.guns[i].level - 1])
                    {
                        upgradeGunButton.GetComponent<Button>().interactable = false;
                    }
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
        stopUseAnimation = true;
        lastCardSelected = (short)i;
        lastTowerClicked = GameManager.instance.GetNonSelectedTowers()[i];
        TowerNotSelectedClicked = (byte)i;
        TowerdetailsPanel.SetActive(true);
        TowerDescription.text = GameManager.instance.GetNonSelectedTowers()[i].description;
        TowerName.text = GameManager.instance.GetNonSelectedTowers()[i].name;
        if (GameManager.instance.GetNonSelectedTowers()[i].Get_damage_player() != 0)
        {
            towerFirePointPanel.SetActive(true);
            TowerDamage.text = GameManager.instance.GetNonSelectedTowers()[i].Get_damage_player().ToString();
        }
        else
        {
            towerDamagePanel.SetActive(false);
        }
        if (GameManager.instance.GetNonSelectedTowers()[i].Get_fireRate_player() != 0)
        {
            towerFirePointPanel.SetActive(true);
            TowerHitSpeed.text = GameManager.instance.GetNonSelectedTowers()[i].Get_fireRate_player().ToString();
        }
        else
        {
            towerFirePointPanel.SetActive(false);
        }
        if (GameManager.instance.GetSelectedTowers()[i].target != "")
        {
            towerTargetPanel.SetActive(true);
            TowerTarget.text = GameManager.instance.GetNonSelectedTowers()[i].target;
        }
        else
        {
            towerTargetPanel.SetActive(false);
        }
        TowerHealth.text = GameManager.instance.GetNonSelectedTowers()[i].Get_health_player().ToString();
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
            if (GameManager.instance.diamond < GameManager.instance.GetNonSelectedTowers()[i].UnlockPrice)
            {
                UnlockTowerButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                UnlockTowerButton.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            UnlockTowerButton.SetActive(false);
            UseTowerButton.SetActive(true);
            upgradeTowerButton.SetActive(true);
            UseTowerButton.GetComponent<Button>().interactable =true;
         
            switch (GameManager.instance.GetNonSelectedTowers()[i].level)
            {
                case 1:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
                    TowerStarlvl1.SetActive(true);
                    TowerStarlvl2.SetActive(false);
                    TowerStarlvl3.SetActive(false);
                    towerUpgradeValue.text = GameManager.instance.GetNonSelectedTowers()[i].UpgradePrice[GameManager.instance.GetNonSelectedTowers()[i].level - 1].ToString();
                    if (GameManager.instance.diamond < GameManager.instance.GetNonSelectedTowers()[i].UpgradePrice[GameManager.instance.GetNonSelectedTowers()[i].level - 1])
                    {
                        upgradeTowerButton.GetComponent<Button>().interactable = false;
                    }
                    break;
                case 2:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
                    TowerStarlvl1.SetActive(false);
                    TowerStarlvl2.SetActive(true);
                    TowerStarlvl3.SetActive(false);
                    towerUpgradeValue.text = GameManager.instance.GetNonSelectedTowers()[i].UpgradePrice[GameManager.instance.GetNonSelectedTowers()[i].level - 1].ToString();
                    if (GameManager.instance.diamond < GameManager.instance.GetNonSelectedTowers()[i].UpgradePrice[GameManager.instance.GetNonSelectedTowers()[i].level - 1])
                    {
                        upgradeTowerButton.GetComponent<Button>().interactable = false;
                    }
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
            TowerHealth.text = GameManager.instance.GetSelectedTowers()[i].Get_health_player().ToString();
            towerImage.sprite = GameManager.instance.GetSelectedTowers()[i].image;

            if (GameManager.instance.GetSelectedTowers()[i].Get_damage_player() != 0)
            {
                towerFirePointPanel.SetActive(true);
                TowerDamage.text = GameManager.instance.GetSelectedTowers()[i].Get_damage_player().ToString();
            }
            else
            {
                towerDamagePanel.SetActive(false);
            }
            if (GameManager.instance.GetSelectedTowers()[i].Get_fireRate_player() != 0)
            {
                towerFirePointPanel.SetActive(true);
                TowerHitSpeed.text = GameManager.instance.GetSelectedTowers()[i].Get_fireRate_player().ToString();
            }
            else
            {
                towerFirePointPanel.SetActive(false);
            }
            if (GameManager.instance.GetSelectedTowers()[i].target != "")
            {
                towerTargetPanel.SetActive(true);
                TowerTarget.text = GameManager.instance.GetSelectedTowers()[i].target;
            }
            else
            {
                towerTargetPanel.SetActive(false);
            }
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
                    if (GameManager.instance.diamond < GameManager.instance.GetSelectedTowers()[i].UpgradePrice[GameManager.instance.GetSelectedTowers()[i].level - 1])
                    {
                        upgradeTowerButton.GetComponent<Button>().interactable = false;
                    }
                    break;
                case 2:
                    upgradeTowerButton.GetComponent<Button>().interactable = true;
                    TowerStarlvl1.SetActive(false);
                    TowerStarlvl2.SetActive(true);
                    TowerStarlvl3.SetActive(false);
                    towerUpgradeValue.text = GameManager.instance.GetSelectedTowers()[i].UpgradePrice[GameManager.instance.GetSelectedTowers()[i].level - 1].ToString();
                    if (GameManager.instance.diamond < GameManager.instance.GetSelectedTowers()[i].UpgradePrice[GameManager.instance.GetSelectedTowers()[i].level - 1])
                    {
                        upgradeTowerButton.GetComponent<Button>().interactable = false;
                    }
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
        SaveSystem.SavePlayer();
        stopUseAnimation = true;
    }
    public void onClickDehors()
    {
        stopUseAnimation = true;

    }
    public void OnUseTowerClick()
    {
        
        TowerdetailsPanel.SetActive(false);
        testTowerClick = true;
        stopUseAnimation = false;
        for (short i = 0; i < 6; i++)
        {
            StartCoroutine(useAnimationSelected(i));
        }
        StartCoroutine(useAnimationNotSelected());
    }

    IEnumerator useAnimationSelected(short nb)
    {
        SelectedTowersPanel.transform.GetChild(nb).GetChild(0).gameObject.SetActive(true);
        while(!stopUseAnimation)
        {
            for (int i = 0; i < 25; i++)
            {
                if (stopUseAnimation)
                {
                    break;
                }
                yield return new WaitForSeconds(1f / 25f);
                float fill = (float)i / 25 ;
                SelectedTowersPanel.transform.GetChild(nb).GetChild(0).GetComponent<Image>().fillAmount = fill;
            }
            for (int i = 0; i < 25; i++)
            {
                if (stopUseAnimation)
                {
                    break;
                }
                yield return new WaitForSeconds(1f / 25f);
                float fill = 1 - ((float)i / 25 );
                SelectedTowersPanel.transform.GetChild(nb).GetChild(0).GetComponent<Image>().fillAmount = fill;
            }
        }
        testTowerClick = false;
        SelectedTowersPanel.transform.GetChild(nb).GetChild(0).gameObject.SetActive(false);
    }
    IEnumerator useAnimationNotSelected()
    {
        short nb = lastCardSelected;
        
        Vector2 startingPos;
        startingPos.x = NotSelectedTowersPanel.transform.GetChild(nb).position.x;
        startingPos.y = NotSelectedTowersPanel.transform.GetChild(nb).position.y;
        
        NotSelectedTowersPanel.transform.GetChild(nb).GetChild(0).gameObject.SetActive(true);
        while(!stopUseAnimation)
        {
            for (int i = 0; i < 25; i++)
            {
                if (stopUseAnimation)
                {
                    break;
                }
               
                NotSelectedTowersPanel.transform.GetChild(lastCardSelected).position = new Vector3(startingPos.x + Mathf.Cos(Time.time * speed) * amount, startingPos.y + Mathf.Sin(Time.time * speed) * amount, NotSelectedTowersPanel.transform.GetChild(lastCardSelected).position.z);


                yield return new WaitForSeconds(1f / 25f);
                float fill = (float)i / 25;
                NotSelectedTowersPanel.transform.GetChild(nb).GetChild(0).GetComponent<Image>().fillAmount = fill;
            }
            for (int i = 0; i < 25; i++)
            {
                if (stopUseAnimation)
                {
                    break;
                }
                NotSelectedTowersPanel.transform.GetChild(lastCardSelected).position = new Vector3(startingPos.x + Mathf.Cos(Time.time * speed) * amount, startingPos.y + Mathf.Sin(Time.time * speed) * amount, NotSelectedTowersPanel.transform.GetChild(lastCardSelected).position.z);
                yield return new WaitForSeconds(1f / 25f);
                float fill = 1 - ((float)i / 25);
                NotSelectedTowersPanel.transform.GetChild(nb).GetChild(0).GetComponent<Image>().fillAmount = fill;
            }
            
        }
        testTowerClick = false;
        NotSelectedTowersPanel.transform.GetChild(nb).position = new Vector3(startingPos.x , startingPos.y , NotSelectedTowersPanel.transform.GetChild(nb).position.z);
        NotSelectedTowersPanel.transform.GetChild(nb).GetChild(0).gameObject.SetActive(false);

    }

    public void OnUseGunClick()
    {
        GameManager.instance.setGun(GameManager.instance.guns[GunClicked]);
        UseGunButton.GetComponent<Button>().interactable = false;
        SaveSystem.SavePlayer();
    }
    public void OnUpgradeTowerClick()
    {
        //GameManager.instance.diamond -= lastTowerClicked.UpgradePrice[lastTowerClicked.level - 1];
        //  gemText.text = GameManager.instance.diamond.ToString();
        //exitTowerDetailsPanel();
        StartCoroutine(CoinAnimation(lastTowerClicked.UpgradePrice[lastTowerClicked.level - 1]));
        lastTowerClicked.level++;

        if (lastTowerClicked.Get_damage_player() != 0)
        {
            towerFirePointPanel.SetActive(true);
            TowerDamage.text = lastTowerClicked.Get_damage_player().ToString();
        }
        else
        {
            towerDamagePanel.SetActive(false);
        }
        if (lastTowerClicked.Get_fireRate_player() != 0)
        {
            towerFirePointPanel.SetActive(true);
            TowerHitSpeed.text = lastTowerClicked.Get_fireRate_player().ToString();
        }
        else
        {
            towerFirePointPanel.SetActive(false);
        }
        
        TowerHealth.text = lastTowerClicked.Get_health_player().ToString();
      
       
        switch (lastTowerClicked.level)
        {
            case 2:
                upgradeTowerButton.GetComponent<Button>().interactable = true;
                towerUpgradeValue.text = lastTowerClicked.UpgradePrice[lastTowerClicked.level - 1].ToString();
                TowerStarlvl1.SetActive(false);
                TowerStarlvl2.SetActive(true);
                TowerStarlvl3.SetActive(false);
                if (GameManager.instance.diamond < lastTowerClicked.UpgradePrice[lastTowerClicked.level - 1])
                {
                    upgradeTowerButton.GetComponent<Button>().interactable = false;
                }
                break;
            case 3:
                upgradeTowerButton.GetComponent<Button>().interactable = false;
                TowerStarlvl1.SetActive(false);
                TowerStarlvl2.SetActive(false);
                TowerStarlvl3.SetActive(true);
                break;
        }
        UpgradeObject(lastTowerClicked.name, lastTowerClicked.image,lastTowerClicked.level);
        SaveSystem.SavePlayer();
    }
    public void OnUpgradeGunClick()
    {
        //exitGunDetailsPanel();
        StartCoroutine(CoinAnimation(GameManager.instance.guns[GunClicked].UpgradePrice[GameManager.instance.guns[GunClicked].level - 1]));
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
                if (GameManager.instance.diamond < GameManager.instance.guns[GunClicked].UpgradePrice[GameManager.instance.guns[GunClicked].level - 1])
                {
                    upgradeGunButton.GetComponent<Button>().interactable = false;
                }
                break;
            case 3:
                upgradeGunButton.GetComponent<Button>().interactable = false;
                GunStarlvl1.SetActive(false);
                GunStarlvl2.SetActive(false);
                GunStarlvl3.SetActive(true);
                break;
        }
        UpgradeObject(GameManager.instance.guns[GunClicked].name, GameManager.instance.guns[GunClicked].image, (byte)(GameManager.instance.guns[GunClicked].level));
        SaveSystem.SavePlayer();
    }
    public void OnUnlockTowerClick()
    {
        //GameManager.instance.diamond -= lastTowerClicked.UnlockPrice;
        // gemText.text = GameManager.instance.diamond.ToString();
        StartCoroutine(CoinAnimation(lastTowerClicked.UnlockPrice));
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
        UnlockObject(lastTowerClicked.name, lastTowerClicked.image);
        //exitTowerDetailsPanel();
        fillTowersSprites();
        SaveSystem.SavePlayer();


    }
    public void OnUnlockGunClick() {
        // GameManager.instance.diamond -= GameManager.instance.guns[GunClicked].UnlockPrice;
        //gemText.text = GameManager.instance.diamond.ToString();
        StartCoroutine(CoinAnimation(GameManager.instance.guns[GunClicked].UnlockPrice));
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
        UnlockObject(GameManager.instance.guns[GunClicked].name, GameManager.instance.guns[GunClicked].image);
        //exitGunDetailsPanel();
        FillGunsSprite();
        SaveSystem.SavePlayer();
    }

    IEnumerator CoinAnimation(short c)
    {
        if (c < 80)
        {
            for (int i = 0; i < c; i++)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                GameManager.instance.diamond -= 1;
                gemText.text = GameManager.instance.diamond.ToString();
            }
        }
        else if (c <= 1200)
        {
            for (int i = 0; i < (int)c / 10; i++)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                GameManager.instance.diamond -= 10;
                gemText.text = GameManager.instance.diamond.ToString();
            }

            GameManager.instance.diamond -= (ushort)(c % 10);
            gemText.text = GameManager.instance.diamond.ToString();
        }
        else
        {
            for (int i = 0; i < (int)c / 100; i++)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                GameManager.instance.diamond -= 100;
                gemText.text = GameManager.instance.diamond.ToString();
            }
            GameManager.instance.diamond -= (ushort)(c % 100);
            gemText.text = GameManager.instance.diamond.ToString();
        }

    }
    IEnumerator CoinAnimationAdd(ushort c)
    {
        if (c < 80)
        {
            for (int i = 0; i < c; i++)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                GameManager.instance.diamond += 1;
                gemText.text = GameManager.instance.diamond.ToString();
            }
        }
        else if(c<=1200)
        {
            for (int i = 0; i <(int) c /10; i++)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                GameManager.instance.diamond += 10;
                gemText.text = GameManager.instance.diamond.ToString();
            }
           
            GameManager.instance.diamond += (ushort)(c % 10);
            gemText.text = GameManager.instance.diamond.ToString();
        }
        else
        {
            for (int i = 0; i < (int)c / 100; i++)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                GameManager.instance.diamond += 100;
                gemText.text = GameManager.instance.diamond.ToString();
            }
            GameManager.instance.diamond += (ushort)(c % 100);
            gemText.text = GameManager.instance.diamond.ToString();
        }
      
    }
}

