using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public short diamond;

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
            GunSelected = guns[0];
            setPlayer(players[0]);
            remplirSelectedTower();
            diamond = 900;
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
