﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{  
    public Text startCoinsTxt;
    private TowerScript[] towers= new TowerScript[6];

    bool  notAlreadyClicked = false;
    private short nb;
    public static short waitBetweenBuild = 7;

    // Start is called before the first frame update
    void Start()
    {
        towers = GetComponent<GameManager>().GetSelectedTowers();
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetMouseButtonDown(0) && notAlreadyClicked)
        {
            testBuilding();
        }
    }

    public void click (int nb)
    {
        notAlreadyClicked = true;       
        this.nb = (short)nb;
        //change sprite to selected
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(3).gameObject.SetActive(true);
        /*for (int i = 0; i < 6; i++)
        {
            if (i == nb)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }*/
    }
    public void unclick()
    {
        notAlreadyClicked = false;
       
            GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(3).gameObject.SetActive(false);
        
    }

    public void testBuilding()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "TowerDefendZone")
            {
                Vector3 towerpos = new Vector3(hit.collider.transform.position.x, transform.position.y, hit.collider.transform.position.z);
                positionManager.add(towers[nb], towerpos);
                //Debug.Log(GameManagerPartie.instance.playerCoins);

            }
            unclick();
        }
    }
}
