using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform zone;
    public TowerScript towerBase;
    // public TowerScript[] towersselected = new TowerScript[6];



    [SerializeField]

    private GameObject itemparent;
    //int i = 0;

    private Transform TowerCanvas;
    // Start is called before the first frame update
    public PlayerScript[] players;
    public GameObject inventory;
    public TowerScript[] Towers;
    public GameObject PlayerSlot;
    public GameObject PlayerSelection;
    private sceneManager sn;
    private TowerScript[] towersSelected = new TowerScript[6];
    private TowerScript[] towersNotSelected;

    void Start()
    {
        //acceding to the scene  manager Script
        towersNotSelected = new TowerScript[(Towers.Length - towersSelected.Length)+1];
        sn = this.GetComponent<sceneManager>();
        remplirSelectedTower();
        Scene currentScene = SceneManager.GetActiveScene();
        switch (currentScene.name)
        {
            case "pvm":
                //getting the towers selected 
                // towersSelected = this.GetComponent<GameManager>().GetSelectedTowers();
                // Debug.Log(towersselected[0].name);
                // getting the item parents to access in it to instatiate the towers that the player can build
                TowerCanvas = GameObject.FindGameObjectWithTag("ItemsParent").transform;
                //instantiate the tower base of the player 
                zone = GameObject.FindGameObjectWithTag("TowerBaseZone").transform;
                Instantiate(towerBase.prefab, zone);
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



                //instantiating the players to player selection menu 
                playerInstantiate();

                for (int i = 0; i < 6; i++)
                {

                    GameObject ChildGameObject1 = inventory.transform.GetChild(i).gameObject;
                    GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
                    GameObject ChildGameObject3 = ChildGameObject2.transform.GetChild(0).gameObject;
                    ChildGameObject3.GetComponent<Image>().sprite = towersSelected[i].image;

                }
                break;
        };


    }
    void Update()
    {

    }
    public TowerScript[] GetSelectedTowers()
    {
        return towersSelected;
    }
    void playerInstantiate()
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
        myButton.onClick.AddListener(() => { sn.OnPlayerClick(obj, i); });

    }
    public void remplirSelectedTower()
    {
        for (int j = 0; j < 6; j++)
        {
            towersSelected[j] = Towers[j];


        }
    }
    private void remplirNotSelectedtowers()
    {

    }
}
