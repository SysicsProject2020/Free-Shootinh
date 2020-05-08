using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem instance;
    public int XP;
    public int currentLevel;
    public Slider fill;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    private void Awake()
    {
        instance = this;
    }


    void Update()
    {
        UpdateXp(5);
    }
    public void UpdateXp(int xp)
    {
        XP += xp;
        
        int curlvl = (int)(0.1f * Mathf.Sqrt(XP));
        if (curlvl != currentLevel)
        {
            currentLevel = curlvl;
            Debug.Log("level UP");
        }

        int xpnextlevel = 100 * (currentLevel + 1) * (currentLevel + 1);
        int differenceXp = xpnextlevel - XP;
        int totaldifference = xpnextlevel - (100 * currentLevel * currentLevel);
        fill.value = 1 -(float)differenceXp / (float)totaldifference;
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
    }
}
