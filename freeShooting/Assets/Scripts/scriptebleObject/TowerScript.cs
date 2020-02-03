using UnityEngine;

[CreateAssetMenu(fileName ="New Tower",menuName ="Tower")]
public class TowerScript: ScriptableObject
{
    public new string name;
    public Sprite image;
    [SerializeField]
    private float[] health ;
    [SerializeField]
    private float[] damage ;
    public GameObject prefab;
    public string description;
    
    [Range(1, 5)]
    public int level;
    
   
    public float Get_health()
    {
        return health[level - 1];
    }
    public float Get_damage()
    {
        return damage[level - 1];
    }

}

  
    
