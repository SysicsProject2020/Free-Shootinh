using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject inventoryPanel;
    public GameObject characterPanel;
    public GameObject SelectGameDiffPanel;
    

    public void playPvm()
    {
        mainPanel.SetActive(false);
        SelectGameDiffPanel.SetActive(true);
    }
    public void playPvp()
    {
        //assign to button when created
        SceneManager.LoadScene("pvp");
    }
    public void option()
    {
        optionPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void exit()
    {
        Application.Quit();
        Debug.Log("Quit !!!");
    }
    public void character()
    {
        mainPanel.SetActive(false);
        characterPanel.SetActive(true);
    }
    public void inventory()
    {
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }
    public void backMainFromInventory()
    {
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }
    public void backMainFromcharacter()
    {
        mainPanel.SetActive(true);
        characterPanel.SetActive(false);
        
    }
    public void backMainFromOption()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);
    }

    public void easy()
    {
        //
        SceneManager.LoadScene("pvm");
    }
    public void medium()
    {
        //
        SceneManager.LoadScene("pvm");
    }
    public void hard()
    {
        //
        SceneManager.LoadScene("pvm");
    }
}
