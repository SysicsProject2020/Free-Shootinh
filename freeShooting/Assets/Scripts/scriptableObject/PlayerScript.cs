using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class PlayerScript : ScriptableObject
{
    private short[] health = { 200, 220, 240, 270, 300 };
    private short[] damage = { 20, 22, 24, 27, 30 };

    public GameObject prefab;
    [Range(1, 5)]
    public byte level;
    public new string name;
    public Sprite image;

    public MagicScript magic1;
    public MagicScript magic2;
    public bool locked = true;

    public short Get_health()
    {
        return health[level - 1];
    }
    public short Get_damage()
    {
        return damage[level - 1];
    }

}
