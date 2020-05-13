using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class PlayerScript : ScriptableObject
{
    private short[] health = { 200, 220, 240, 270, 300 };
    private short[] damage = { 20, 22, 24, 27, 30 };

    public GameObject prefab;
    [Range(1, 3)]
    public byte level;
    public new string name;
    public Sprite image;
    public Sprite Lockedimage;

    public MagicScript magic1;
    public MagicScript magic2;
    public bool locked = true;
    public short[] UpgradePrice = new short[3];
    public short UnlockPrice;
    public short Get_health_player()
    {
        return health[level - 1];
    }
    public short Get_damage_player()
    {
        return damage[level - 1];
    }

    public short Get_health_enemy(byte lvl)
    {
        return health[lvl - 1];
    }
    public short Get_damage_enemy(byte lvl)
    {
        return damage[lvl - 1];
    }
}
