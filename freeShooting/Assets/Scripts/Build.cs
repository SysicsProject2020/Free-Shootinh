using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField]
    private GameObject Towerscanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if (hit.collider.tag == "TowerDefendZone")
            {
                Towerscanvas.SetActive(true);
                
            }


        }
    }
    public void exit()
    {
        Towerscanvas.SetActive(false);
    }
}
