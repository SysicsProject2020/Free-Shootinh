 using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerScript[] players;
    public GameObject inventory;
    public TowerScript[] Towers;
    public GameObject PlayerSlot;
    public GameObject PlayerSelection;
    private sceneManager sn;
    private TowerScript[] towersSelected=new TowerScript[6];

    void Start()
    {
        //acceding to the scene  manager Script
        sn = this.GetComponent<sceneManager>();
        remplirSelectedTower();
        Scene currentScene = SceneManager.GetActiveScene();
        switch(currentScene.name)
        {
            case "Game":

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
            GameObject go = Instantiate(PlayerSlot, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            go.transform.SetParent(PlayerSelection.transform);
            GameObject ChildGameObject1 = go.transform.GetChild(0).gameObject;
            GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
            ChildGameObject2.GetComponent<Image>().sprite = players[i].image;
            RegisterListener(ChildGameObject1, i);
        }
    }
    public void RegisterListener(GameObject obj,int i)
    {
        Button myButton = obj.GetComponent<Button>();
        myButton.onClick.AddListener(() => { sn.OnPlayerClick(obj,i); });
        
    }
    public void remplirSelectedTower()
    {
        for (int j = 0; j < 6; j++)
        {
            towersSelected[j] = Towers[j];
            
            
        }
    }
}
