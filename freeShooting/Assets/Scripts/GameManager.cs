 using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public PlayerScript[] players;


   
    [SerializeField]
    private TowerScript[] Towers;
    public GameObject PlayerSlot;
    public GameObject PlayerSelection;
    private SceneManager sn;
   



    // Start is called before the first frame update
    void Start()
    {
        sn = this.GetComponent<SceneManager>();
        playerInstantiate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public TowerScript[] GetSelectedTowers()
    {
        return Towers;
    }
    void playerInstantiate()
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameObject go = Instantiate(PlayerSlot, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            go.transform.parent = PlayerSelection.transform;
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
}
