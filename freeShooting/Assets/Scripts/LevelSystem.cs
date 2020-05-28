using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem instance;
    public GameObject levelUpPanel;
    public TextMeshProUGUI congratsTxt;
    
    public Slider fill;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI currentText;
    int differenceXp ;
    int xpnextlevel ;
    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        ADDxp();
    }
    void Update()
    {
        //UpdateXp(5);
    }
    public void exitLevelUp()
    {
        levelUpPanel.SetActive(false);
        GetComponent<MenuManager>().changeHeroMain();
    }
    public void ADDxp()
    {
       
        int curlvl = (int)(0.1f * Mathf.Sqrt(GameManager.instance.XP));
       

        
            if (GameManager.instance.testLevelUp)
            {
                GetComponent<MenuManager>().HeroMain.SetActive(false);
                levelUpPanel.SetActive(true);
                congratsTxt.text = "Congratulation!! <br> You passed to level " + GameManager.instance.CurrentLevel + " !!";
                GameManager.instance.diamond += 5;
                GetComponent<MenuManager>().gemText.text = GameManager.instance.diamond.ToString();
                SaveSystem.SavePlayer();
                GameManager.instance.testLevelUp = false;
                Debug.Log("level UP");
            }
           

        

        xpnextlevel = 100 * (GameManager.instance.CurrentLevel + 1) * (GameManager.instance.CurrentLevel + 1);
        differenceXp = xpnextlevel - GameManager.instance.XP;
        int totaldifference = xpnextlevel - (100 * GameManager.instance.CurrentLevel * GameManager.instance.CurrentLevel);
        fill.value = 1 - (float)differenceXp / (float)totaldifference;
        currentLevelText.text = GameManager.instance.CurrentLevel.ToString();
        nextLevelText.text = (GameManager.instance.CurrentLevel + 1).ToString();
        currentText.text = GameManager.instance.XP.ToString() + "/" + xpnextlevel.ToString();


    }
}
