using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData  
{
    public string SelectedPlayer;
    public string[] SelectedTowers=new string[6];
   // public string[] UnlockedPlayers = new string[GameManager.instance.players.Length];
   // public string[] Unlockedtowers =new string[GameManager.instance.Towers.Length];
    public byte[] towersLevel = new byte[GameManager.instance.Towers.Length];
    public byte[] playersLevel=new byte[GameManager.instance.players.Length];
    public bool[] lockPlayersData=new bool[GameManager.instance.players.Length];
    public bool[] lockTowersData=new bool[GameManager.instance.Towers.Length];


    public PlayerData()
    {
        SelectedPlayer = GameManager.instance.getPlayer().name;
        for (int i = 0; i < GameManager.instance.Towers.Length;i++)
        {
            //SelectedTowers[i] = GameManager.instance.GetSelectedTowers()[i].name;
            // Unlockedtowers[i] = GameManager.instance.GetSelectedTowers()[i].name;
            // towersUnlockedLevel[i] = GameManager.instance.GetSelectedTowers()[i].level;
            //Debug.Log(SelectedTowers[i]);
            lockTowersData[i] = GameManager.instance.Towers[i].locked;
            towersLevel[i] = GameManager.instance.Towers[i].level;
        }
        for(int k=0;k<6;k++)
        {
            SelectedTowers[k] = GameManager.instance.GetSelectedTowers()[k].name;
        }
        for (int j = 0; j < GameManager.instance.players.Length;j++)
        {
            /* if(GameManager.instance)
             UnlockedPlayers[0] = GameManager.instance.players[0].name;
             playersunlockedLevel[0] = GameManager.instance.players[0].level;*/
            lockPlayersData[j] = GameManager.instance.players[j].locked;
            playersLevel[j] = GameManager.instance.players[j].level;
        }



    }
    
}
