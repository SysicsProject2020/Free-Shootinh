using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerScript[] players; 



    [SerializeField]
    private TowerScript[] Towers;
    public GameObject PlayerSlot;
   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public TowerScript[] GetSelectedTowers()
    {
        return Towers;
    }

}
