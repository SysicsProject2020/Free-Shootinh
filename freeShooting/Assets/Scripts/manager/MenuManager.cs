using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    private bool shake = false;
    Vector2 startingPos;
    float speed = 10f; //how fast it shakes
    float amount = 10f;
    private int k = 0;
  

    public Button backMainFromInventoryBtn;
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject inventoryPanel;
    public GameObject characterPanel;
    public GameObject SelectGameDiffPanel;
    public Button backMainFromcharacterBtn;
    public GameObject PlayerdetailsPanel;
    public GameObject PlayerSlot;
    public GameObject PlayerSelection;
    public GameObject towerSlot;
    public GameObject inventory;
    private bool testClick=false;
    private int TowerNotSelectedClicked;
    public GameObject towerdetailsPanel;
    public GameObject towerNotSelectedMenu;
    private TowerScript[] towersSelected = new TowerScript[6];
    private TowerScript[] towersNotSelected;
    private TMPro.TextMeshProUGUI txtTowerDetails;
    private TMPro.TextMeshProUGUI txtPlayerDetails;



    public GameObject UnlockPlayerBtn;
    public GameObject UsePlayerButton;
    public GameObject UnlockTowerBtn;
    public GameObject useTowerButton;
    public GameObject upgradePlayersBtn;
    public GameObject upgradeTowersBtn;


    private void Awake()
    {
        instance = this;
        towersNotSelected = GameManager.instance.GetNonSelectedTowers();
        towersSelected = GameManager.instance.GetSelectedTowers();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        txtTowerDetails = towerdetailsPanel.transform.Find("description").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        txtPlayerDetails = PlayerdetailsPanel.transform.Find("description").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        startingPos.x = inventory.transform.position.x;
        startingPos.y = inventory.transform.position.y;
        playerMenuInstantiate();
        towersMenuInstantiate();
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
        myButton.onClick.AddListener(() => { MenuManager.instance.OnChoosingCards(i); });
    }
    public void fillSprites()
    {
        for (int i = 0; i < 6; i++)
        {

            inventory.transform.GetChild(i).GetComponentInChildren<Image>().sprite = towersSelected[i].image;

        }
        for (int j = 0; j < towersNotSelected.Length; j++)
        {
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
        
         if(testClick==true)
        {
            SwitchTowers(i, TowerNotSelectedClicked);
            fillSprites();
            shake = false;
            inventory.transform.position = startingPos;
            testClick = false;
        }
        else
        {
            towerdetailsPanel.SetActive(true);
            backMainFromInventoryBtn.GetComponent<Button>().interactable = false;
            txtTowerDetails.text = "Tower Name : " + towersSelected[i].name;
            towerdetailsPanel.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = false;
        }
    }
    public void OnChoosingCards(int i)
    {
        towerdetailsPanel.SetActive(true);
        backMainFromInventoryBtn.GetComponent<Button>().interactable = false;

        shake = false;
        txtTowerDetails.text = "Tower Name : " + towersNotSelected[i].name ;
        towerdetailsPanel.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = true;
        TowerNotSelectedClicked = i;
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

        k = i;
        PlayerdetailsPanel.SetActive(true);
        backMainFromcharacterBtn.interactable = false;
        for(int j=0;j<GameManager.instance.players.Length;j++)
        {
            if (GameManager.instance.players[k] == GameManager.instance.getPlayer())
            {
                UsePlayerButton.SetActive(true);
                UsePlayerButton.GetComponent<Button>().interactable = false;
                UnlockPlayerBtn.SetActive(false);
                if (GameManager.instance.players[k].level < 5)
                {
                    upgradePlayersBtn.GetComponent<Button>().interactable = true;
                }
                else { upgradePlayersBtn.GetComponent<Button>().interactable = false; }

            }
            else if (GameManager.instance.players[k].locked == true)
            {
                UsePlayerButton.SetActive(false);
                UnlockPlayerBtn.SetActive(true);
                upgradePlayersBtn.GetComponent<Button>().interactable = false;
            }
            else if(GameManager.instance.players[k].locked==false)
            {
                UsePlayerButton.GetComponent<Button>().interactable = true;
                UsePlayerButton.SetActive(true);
                UnlockPlayerBtn.SetActive(false);
                if (GameManager.instance.players[k].level < 5)
                {
                    upgradePlayersBtn.GetComponent<Button>().interactable = true;
                }else { upgradePlayersBtn.GetComponent<Button>().interactable = false; }
            }
            
        }
        txtPlayerDetails.text = "Character Name : " + GameManager.instance.players[k].name + " Magic1 description" + GameManager.instance.players[k].magic1.description + "level = " + GameManager.instance.players[k].level ;

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
    public void UseButton() {
        GameManager.instance.setPlayer(GameManager.instance.players[k]);
        PlayerdetailsPanel.SetActive(false);
        backMainFromcharacterBtn.interactable = true;
        //Debug.Log(GameManager.instance.getPlayer().name);


    }
    public void UnlockPlayerButton()
    {
        GameManager.instance.players[k].locked = false;
        PlayerdetailsPanel.SetActive(false);
        backMainFromcharacterBtn.interactable = true;
    }
    public void UpgradeTowerBtn() { }
    public void UpgradePlayerBtn() {
        if (GameManager.instance.players[k].level <= 5) { GameManager.instance.players[k].level++; }
        backMainFromcharacterBtn.interactable = true;
    }
    public void UpgradeTowerNotSelectedBtn() { }
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
/*public void OnPlayerNotClicked()
  {
      if (Object != null)
      {
          GameObject ChildGameObject1 = Object.transform.GetChild(1).gameObject;
          GameObject ChildGameObject2 = Object.transform.GetChild(2).gameObject;
          ChildGameObject1.SetActive(false);
          ChildGameObject2.SetActive(false);

      }
  }*/
