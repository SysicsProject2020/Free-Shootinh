using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class PlayerScript : ScriptableObject
{
    private int[] health = { 200, 220, 240, 270, 300 };
    private int[] damage = { 20, 22, 24, 27, 30 };

    public GameObject prefab;
    [Range(1, 5)]
    public int level;
    public new string name;
    public Sprite image;

    public MagicScript magic1;
    public MagicScript magic2;

    public int Get_health()
    {
        return health[level - 1];
    }
    public int Get_damage()
    {
        return damage[level - 1];
    }

}
