using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Sprite[] Pictures;
    public short winCount;
    public short loseCount;
    public short damageDone;
    public short gamePlayed;
    public byte playerPicture;
    public short diamond;
    public string playerName;
    public GunsScript[] guns = new GunsScript[10];   
    public static GameManager instance;
    public byte TowersNumber;
    public PlayerScript[] players;
    public GameObject inventory;
    public TowerScript[] Towers;
    private PlayerScript playerSelected;
    private GunsScript GunSelected;
    private TowerScript[] towersSelected = new TowerScript[6];
    private TowerScript[] towersNotSelected;
    public GameObject[] bases= new GameObject[3];
    public int XP;
    // public PlayerData data;
    [Header("shop")]
    public GemScript[] packs = new GemScript[4];

    private void Awake()
    {
        instance = this;
        towersNotSelected = new TowerScript[(Towers.Length - towersSelected.Length)];
        if(SaveSystem.testExist())
        {
            loadData();
        }
        else
        {
            winCount = 0;
            loseCount = 0;
            damageDone = 0;
            gamePlayed = 0;
            playerPicture = 0;
            playerName = "player(" + UnityEngine.Random.Range(1, 255) + ")";
            GunSelected = guns[0];
            setPlayer(players[0]);
            remplirSelectedTower();
            players[0].level = 1;
            players[0].locked = false;
            for(int i = 1; i < players.Length; i++){
                players[i].level = 1;
                players[i].locked = true;

            }
            guns[0].level = 1;
            guns[0].locked = false;
            for (int i = 1; i < guns.Length; i++)
            {
                guns[i].locked = true;
                guns[i].level = 1;
            }
            
            for(int i = 0; i<Towers.Length; i++)
            {
                Towers[i].level = 1;
                if (i > 5)
                {
                    Towers[i].locked = true;
                }
                else
                {
                    Towers[i].locked = false;
                }
            }
            diamond = 0;
            TowersNumber = 9;
            XP = 0;
            SaveSystem.SavePlayer();
        }
        FillTowersNotSelected();
        //Debug.Log(getGun().name);
    }
    private void loadData()
    {
        PlayerData data = SaveSystem.loadPlayerData();
        winCount = data.winCount;
        loseCount = data.loseCount;
        damageDone = data.damageDone;
        gamePlayed = data.gamePlayed;
        playerPicture = data.playerPicture;
        playerName = data.PlayerName;
        diamond = data.diamonds;
        for (int i=0;i<players.Length;i++)
        {
            if(players[i].name==data.SelectedPlayer)
            {
                playerSelected = players[i];
            }
            players[i].locked = data.lockPlayersData[i];
            players[i].level = data.playersLevel[i];
        }
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i].name == data.selectedGun)
            {
                GunSelected = guns[i];
            }
            guns[i].locked = data.gunsLocked[i];
            guns[i].level = data.gunsLevel[i];
        }
        int k = 0;
        for(int i=0;i<TowersNumber; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                if(Towers[i].name==data.SelectedTowers[j])
                {
                    towersSelected[k] = Towers[i];
                    k++;
                    break;
                }
            }
            Towers[i].locked = data.lockTowersData[i];
            Towers[i].level = data.towersLevel[i];
        }
        if (Towers.Length > TowersNumber)
        {
            for (int i = TowersNumber; i < Towers.Length; i++)
            {
                Towers[i].locked = true;
                Towers[i].level = 1;
            }
            TowersNumber = (byte)Towers.Length;
        }
        //Debug.Log(TowersNumber);
            XP = data.XP;
    }
    public PlayerScript getPlayer()
    {
        return playerSelected;
    }
    public void setPlayer(PlayerScript Player)
    {
        playerSelected = Player;
    }
    public void UpdateXp(int xp)
    {
        XP += xp;
        SaveSystem.SavePlayer();

    }
    private void FillTowersNotSelected()
    {
        int h = 0;
        bool test;
        for(int i=0;i<Towers.Length;i++)
        {
            test = true;
            for (int j = 0; j<towersSelected.Length;j++)
            {
                if (Towers[i] == towersSelected[j])
                {
                    test = false ;
                    break;
                }
            }
            if (test)
            {
                towersNotSelected[h] = Towers[i];
                h++;
            }
        }
       
    }
    public TowerScript[] GetSelectedTowers()
    {
        return towersSelected;
    }
    public TowerScript[] GetNonSelectedTowers()
    {
        return towersNotSelected;
    }
    public void remplirSelectedTower()
    {
        for (int j = 0; j < 6; j++)
        {
            towersSelected[j] = Towers[j];
        }
    }
    public void  setSelectedTower(TowerScript[] towers)
    {
        towersSelected = towers;
    }
    public GunsScript getGun()
    {
        return GunSelected;
    }
    public void setGun(GunsScript gun)
    {
        GunSelected = gun;
    }

}
