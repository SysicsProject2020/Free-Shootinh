using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem instance;
    
    public int currentLevel;
    public Slider fill;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI currentText;
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

    public void ADDxp()
    {
        int curlvl = (int)(0.1f * Mathf.Sqrt(GameManager.instance.XP));
        if (curlvl != currentLevel)
        {
            currentLevel = curlvl;
            Debug.Log("level UP");
        }

        int xpnextlevel = 100 * (currentLevel + 1) * (currentLevel + 1);
        int differenceXp = xpnextlevel - GameManager.instance.XP;
        int totaldifference = xpnextlevel - (100 * currentLevel * currentLevel);
        fill.value = 1 - (float)differenceXp / (float)totaldifference;
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
        currentText.text = GameManager.instance.XP.ToString() + "/" + xpnextlevel.ToString();


    }
}
