using UnityEngine;

[CreateAssetMenu(fileName ="New Tower",menuName ="Tower")]
public class TowerScript: ScriptableObject
{
    public new string name;
    public Sprite image;
    public Sprite Lockedimage;
    [SerializeField]
    private short[] health =new short[3] ;
    [SerializeField]
    private short[] damage = new short[3];
    public float fireRate; 
    public GameObject prefab;
    public string description;
    public bool locked = true;
    public string target;
    [Range(1, 3)]
    public byte level;
    
    public short[] UpgradePrice = new short[3];
    public short cost;
    public short UnlockPrice;


    public short Get_health_player()
    {
        return health[level-1];
    }
    public short Get_damage_player()
    {
        return damage[level - 1];
    }

    public float Get_fireRate_player()
    {
        //return fireRate[level - 1];
        return fireRate;
    }

    public short Get_health_enemy(byte lvl)
    {
        return health[lvl - 1];
    }

    public short Get_damage_enemy(byte lvl)
    {
        return damage[lvl - 1];
    }

    public float Get_fireRate_enemy(byte lvl)
    {
        //return fireRate[lvl - 1];
        return fireRate;
    }
}

  
    
