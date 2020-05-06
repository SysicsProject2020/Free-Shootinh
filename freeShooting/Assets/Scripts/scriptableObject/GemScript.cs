using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gem", menuName = "Gem")]
public class GemScript : ScriptableObject
{
    public short gemCount;
    public Sprite image;
    public float price;
    public float onSalePercentage;
}
