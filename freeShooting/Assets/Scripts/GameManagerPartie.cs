using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPartie : MonoBehaviour
{
    public Transform zone;
    public TowerScript towerBase;
    public TowerScript[] towersselected=new TowerScript[6];
    
    
    
    [SerializeField]
   
    private GameObject itemparent;
    //int i = 0;

    private Transform TowerCanvas;
    // Start is called before the first frame update
    void Start()
    {
        //getting the towers selected 
        towersselected = this.GetComponent<GameManager>().GetSelectedTowers();
        Debug.Log(towersselected[0].name);
        // getting the item parents to access in it to instatiate the towers that the player can build
        TowerCanvas = GameObject.FindGameObjectWithTag("ItemsParent").transform;
        //instantiate the tower base of the player 
        zone = GameObject.FindGameObjectWithTag("TowerBaseZone").transform;
        Instantiate(towerBase.prefab,zone);
        // instantiate towers that the player can build


        for (int i = 0; i < 6; i++)
        {

            GameObject ChildGameObject1 = itemparent.transform.GetChild(i).gameObject;
            GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
            GameObject ChildGameObject3 = ChildGameObject2.transform.GetChild(0).gameObject;
            //ChildGameObject3.GetComponent<Image>().sprite = towersselected[i].image;


        }




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
