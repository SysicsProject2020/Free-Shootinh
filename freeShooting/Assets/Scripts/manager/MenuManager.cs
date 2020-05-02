using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuManager : MonoBehaviour
{    
    public GameObject mainPanel;
    public GameObject settingPanel;
    public GameObject inventoryPanel;
    public GameObject shopPanel;
    
    public void playPvm()
    {
        SceneManager.LoadScene("pvm");
    }
    public void setting()
    {      
        settingPanel.SetActive(true);       
    }
    public void exitSetting()
    {
        settingPanel.SetActive(false);
    }
    public void shop()
    {        
        shopPanel.SetActive(true);
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }
    public void main()
    {
        shopPanel.SetActive(false);
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }
    public void inventory()
    {
        shopPanel.SetActive(false);
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }
}

