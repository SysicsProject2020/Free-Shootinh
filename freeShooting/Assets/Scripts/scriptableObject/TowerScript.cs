using UnityEngine;

[CreateAssetMenu(fileName ="New Tower",menuName ="Tower")]
public class TowerScript: ScriptableObject
{
    public new string name;
    public Sprite image;
    [SerializeField]
    private short[] health =new short[5] ;
    [SerializeField]
    private short[] damage = new short[5];
    public GameObject prefab;
    public string description;
    public bool locked = true;
    /*
    //1 : in front  
    //2 : 3 9demou
    //3 : all except player
    //4 : all, player
    [Range(1, 4)]
    public short range;
    */
    
    [Range(1, 5)]
    public byte level;
    public short cost;
    public short UnlockPrice;


    public short Get_health()
    {
        return health[level-1];
    }
    public short Get_damage()
    {
        return damage[level - 1];
    }

}

  
    
