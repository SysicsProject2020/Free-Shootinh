using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData  
{
    public string SelectedPlayer;
    public string[] SelectedTowers=new string[6];
    public byte diamonds;
    public int XP;
    public byte playersNumber;
    public byte towersNumber;
    public string selectedGun;
    public byte[] gunsLevel = new byte[GameManager.instance.guns.Length];
    public bool[] gunsLocked = new bool[GameManager.instance.guns.Length];
    // public string[] UnlockedPlayers = new string[GameManager.instance.players.Length];
    // public string[] Unlockedtowers =new string[GameManager.instance.Towers.Length];
    public byte[] towersLevel = new byte[GameManager.instance.Towers.Length];
    public byte[] playersLevel=new byte[GameManager.instance.players.Length];
    public bool[] lockPlayersData=new bool[GameManager.instance.players.Length];
    public bool[] lockTowersData=new bool[GameManager.instance.Towers.Length];


    public PlayerData()
    {
        diamonds = GameManager.instance.diamond;
        XP = LevelSystem.instance.XP;
        selectedGun = GameManager.instance.getGun().name;
        SelectedPlayer = GameManager.instance.getPlayer().name;
        for (int i = 0; i < GameManager.instance.Towers.Length;i++)
        {
            lockTowersData[i] = GameManager.instance.Towers[i].locked;
            towersLevel[i] = GameManager.instance.Towers[i].level;
        }
        for(int i = 0; i < GameManager.instance.guns.Length; i++)
        {
            gunsLevel[i] = GameManager.instance.guns[i].level;
            gunsLocked[i] = GameManager.instance.guns[i].locked;
        }
        for(int k=0;k<6;k++)
        {
            SelectedTowers[k] = GameManager.instance.GetSelectedTowers()[k].name;
            //Debug.Log(SelectedTowers[k]);
        }
        for (int j = 0; j < GameManager.instance.players.Length;j++)
        {
            lockPlayersData[j] = GameManager.instance.players[j].locked;
            playersLevel[j] = GameManager.instance.players[j].level;
        }
        towersNumber = GameManager.instance.TowersNumber;



    }
    public void addTower()
    {
        for (int i = 0; i < GameManager.instance.Towers.Length; i++)
        {
            lockTowersData[i] = GameManager.instance.Towers[i].locked;
            towersLevel[i] = GameManager.instance.Towers[i].level;
        }
        SaveSystem.SavePlayer();
    }
}
