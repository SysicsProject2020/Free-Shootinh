using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public TowerScript towerBase;
    public TowerScript enemybase;
    public PlayerScript player;
    public PlayerScript enemy;
    private Vector3 playerPos = new Vector3(0, 0.74f, -14);
    private Vector3 enemypos = new Vector3(0, 0.74f, 14);
    private Vector3 playerTowerPos = new Vector3(0,2, -18);
    private Vector3 enemyTowerPos = new Vector3(0,2, 18);





    
    // public TowerScript[] towersselected = new TowerScript[6];



    [SerializeField]

    private GameObject itemparent;
    //int i = 0;

    private Transform TowerCanvas;
    public PlayerScript[] players;
    public GameObject inventory;
    
    public TowerScript[] Towers;
    public GameObject PlayerSlot;
    public GameObject PlayerSelection;
    private sceneManager sn;
    private TowerScript[] towersSelected = new TowerScript[6];
    private TowerScript[] towersNotSelected;
    public GameObject towerNotSelectedMenu;
    public GameObject towerSlot ;
    private MenuManager Menumanager;


    void Start()
    {




       
        Menumanager = this.GetComponent<MenuManager>();
        towersNotSelected = new TowerScript[(Towers.Length - towersSelected.Length)];
        //acceding to the scene  manager Script
        sn = this.GetComponent<sceneManager>();
        remplirSelectedTower();
        FillTowersNotSelected();
        Scene currentScene = SceneManager.GetActiveScene();
        switch (currentScene.name)
        {
            case "pvm":
              towerBase.prefab.GetComponent<target>().health = towerBase.Get_health();
              enemybase.prefab.GetComponent<target>().health = enemybase.Get_health();
                player.prefab.GetComponent<gun>().damage = player.Get_damage();
                enemy.prefab.GetComponent<gun>().damage = enemy.Get_damage();
                







                // getting the item parents to access in it to instatiate the towers that the player can build
                TowerCanvas = GameObject.FindGameObjectWithTag("ItemsParent").transform;
                //instantiate the tower base of the player 
               
                 Instantiate(towerBase.prefab,playerTowerPos,Quaternion.Euler(0,0,0));
                 Instantiate(enemybase.prefab, enemyTowerPos, Quaternion.Euler(-180, 0, 0));

                Instantiate(player.prefab, playerPos, Quaternion.Euler(0, 0, 0));
                Instantiate(enemy.prefab, enemypos, Quaternion.Euler(-180, 0, 0));


                // instantiate towers that the player can build


                for (int i = 0; i < 6; i++)
                {

                    GameObject ChildGameObject1 = itemparent.transform.GetChild(i).gameObject;
                    GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
                    GameObject ChildGameObject3 = ChildGameObject2.transform.GetChild(0).gameObject;
                    ChildGameObject3.GetComponent<Image>().sprite = towersSelected[i].image;


                }
             
                break;
            case "menu":



                
               
                playerMenuInstantiate();
                towersMenuInstantiate();
                fillSprites();


                break;

        };


    }
    public void towersMenuInstantiate()
    {
       
        for (int j = 0; j < towersNotSelected.Length; j++)
        {
            GameObject go = Instantiate(towerSlot, towerNotSelectedMenu.transform);
            GameObject ChildGameObject2 = go.transform.GetChild(0).gameObject;
            RegisterListenerTowerSwitch(ChildGameObject2, j);
        }
    }
    public void fillSprites()
    {
        for (int i = 0; i < 6; i++)
        {

            GameObject ChildGameObject1 = inventory.transform.GetChild(i).gameObject;
            GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
            GameObject ChildGameObject3 = ChildGameObject2.transform.GetChild(0).gameObject;
            ChildGameObject3.GetComponent<Image>().sprite = towersSelected[i].image;

        }
        for (int j = 0; j < towersNotSelected.Length; j++)
        {
            GameObject ChildGameObject1 = towerNotSelectedMenu.transform.GetChild(j).gameObject;
            GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
            GameObject ChildGameObject3 = ChildGameObject2.transform.GetChild(0).gameObject;
            ChildGameObject3.GetComponent<Image>().sprite = towersNotSelected[j].image;
        }
    }
    
    void Update()
    {
        

      
    }
    private void FillTowersNotSelected()
    {
        int k = 0;
        bool test;
        for(int i=0;i<Towers.Length;i++)
        {
            test = true;
            for (int j = 0; j<towersSelected.Length;j++)
            {
                //Debug.Log(i);
                if (Towers[i] == towersSelected[j])
                {
                    test = false ;
                    break;
                    
                }
                

            }
            if (test)
            {
       
                towersNotSelected[k] = Towers[i];
 
                k++;
                

            }
        }
    }
    public TowerScript[] GetSelectedTowers()
    {
        return towersSelected;
    }
    public TowerScript[] GetNonSelectedTowers()
    {
        return towersNotSelected;
    }
    void playerMenuInstantiate()
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameObject go = Instantiate(PlayerSlot, PlayerSelection.transform.position, Quaternion.identity) as GameObject;

            go.transform.SetParent(PlayerSelection.transform);
            GameObject ChildGameObject1 = go.transform.GetChild(0).gameObject;
            GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
            ChildGameObject2.GetComponent<Image>().sprite = players[i].image;
            RegisterListener(ChildGameObject1, i);
        }
     
    }
    public void RegisterListener(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => { sn.OnPlayerClick(i); });

    }
    public void RegisterListenerTowerSwitch(GameObject obj, int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => {Menumanager.OnChoosingCards(i); });
       
    }
    public void remplirSelectedTower()
    {
        for (int j = 0; j < 6; j++)
        {
            towersSelected[j] = Towers[j];
        }
    }
    public void SwitchTowers (int i,int j)
    {
        TowerScript tower = towersSelected[i];
        towersSelected[i] = towersNotSelected[j];
        towersNotSelected[j] = tower;
       

    }
    

    
   
}
