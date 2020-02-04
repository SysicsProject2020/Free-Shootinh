using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Build : MonoBehaviour
{
    RaycastHit hit;
    GameManagerPartie gm;
    private TowerScript[] towers= new TowerScript[6];
    
    bool  test = false;
    private int nb;
    
    // Start is called before the first frame update
    void Start()
    {
        towers = this.GetComponent<GameManager>().GetSelectedTowers();
        
        
       // Debug.Log(gm.towersselected[0]);

    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetMouseButtonDown(0) && test == true)
        {
            testBuilding();


        }
    }

    public void click (int nb)
    {
        test = true;
        
        this.nb = nb;
    }
    public void testBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "TowerDefendZone")
            {

                GameObject go = Instantiate(towers[nb].prefab, hit.transform);
                go.transform.localScale = new Vector3(7, 7, 7);
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 0.5f, go.transform.position.z);
                test = false;
            }
            else
                test = false;
        }
    }
    
}
