using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    //GameObject currenSelectedObj=null;
    GameObject Object=null;
    private  int k = 0;
    public GameObject backBtn;
    private GameManager GM ;
    public GameObject DetailsPanel;
    // Start is called before the first frame update
    void Start()
    {
        GM = this.GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {


        /*currenSelectedObj = EventSystem.current.currentSelectedGameObject;
        if ((currenSelectedObj != Object) ) 
            {
                
                    OnPlayerNotClicked();
                    Object = currenSelectedObj;
                
            }*/
        
           
        
    }
    public void OnPlayerClick (GameObject obj, int i)
    {
       /* GameObject ChildGameObject1 = obj.transform.GetChild(1).gameObject;
        GameObject ChildGameObject2 = obj.transform.GetChild(2).gameObject;
        ChildGameObject1.SetActive(true);
        ChildGameObject2.SetActive(true);*/
        k = i;
        DetailsPanel.SetActive(true);
        backBtn.GetComponent<Button>().interactable = false;
        GameObject desc = DetailsPanel.transform.Find("description").gameObject;
        TMPro.TextMeshProUGUI txt = desc.GetComponent<TMPro.TextMeshProUGUI>();
        txt.text = "Character Name : " + GM.players[k].name + " Magic1 description" + GM.players[k].magic1.description;

    }
    public void OnPlayerNotClicked()
    {
        if (Object != null)
        {
            GameObject ChildGameObject1 = Object.transform.GetChild(1).gameObject;
            GameObject ChildGameObject2 = Object.transform.GetChild(2).gameObject;
            ChildGameObject1.SetActive(false);
            ChildGameObject2.SetActive(false);
        
        }
    }
    public void UseButton() { }
    public void DetailsButton() {
        
        
    }
    public void back()
    {
        //currenSelectedObj.SetActive(false);
        DetailsPanel.gameObject.SetActive(false);
        backBtn.GetComponent<Button>().interactable = true;




    }
}
