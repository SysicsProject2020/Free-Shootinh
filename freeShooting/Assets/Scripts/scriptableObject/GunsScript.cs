
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunsScript : ScriptableObject
{
    //public GameObject prefab;
    public new string name;
    public string description;
    public Sprite image;
    public GameObject prefab;
    public GameObject GunBullet;
    public short speed;
    public Transform firePoint;
    public bool locked = true;
    [SerializeField]
    private float[] fireRate=new float[5];
    [SerializeField]
    private short[] damage = new short[5];
    [Range(1, 5)]
    public byte level;
    public short UnlockPrice;

    public short Get_damage_Gun_player()
    {
        return damage[level - 1];
    }
    public float Get_fireRate_Gun_player()
    {
        return fireRate[level - 1];
        //return fireRate;
    }

    public short Get_damage_Gun_enemy(byte lvl)
    {
        return damage[lvl - 1];
    }

    public float Get_fireRate_Gun_enemy(byte lvl)
    {
        return fireRate[lvl - 1];
        //return fireRate;
    }
}





