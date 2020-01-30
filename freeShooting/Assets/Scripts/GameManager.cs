using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform zone;
    public TowerScript tower;

    // Start is called before the first frame update
    void Start()
    {
        zone = GameObject.FindGameObjectWithTag("TowerBaseZone").transform;
        
        Instantiate(tower.obj,zone);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
