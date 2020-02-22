using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem instance;
    public int XP;
    public int currentLevel;
    public GameObject XpBar;
    public Image fill;
    public Text XPtxt;
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
        }
        int xpnextlevel = 100 * (currentLevel + 1) * (currentLevel + 1);
        int differenceXp = xpnextlevel - XP;
        int totaldifference = xpnextlevel - (100 * currentLevel * currentLevel);
        fill.fillAmount = (float)differenceXp / (float)totaldifference;
        XPtxt.text = (XP +"/"+ xpnextlevel).ToString();
    }
}
