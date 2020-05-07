using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class packInf : MonoBehaviour
{
    public TextMeshProUGUI gemCount;
    public Image sprite;
    public TextMeshProUGUI price;
    public TextMeshProUGUI sale;
    public GameObject saleDel;

    public void packwrite(short gemCount,Sprite image,float price,float onSalePercentage)
    {
        this.gemCount.text = gemCount.ToString();
        sprite.sprite = image;
        this.price.text = price.ToString();
        if (onSalePercentage == 0)
        {
            saleDel.SetActive(false);
        }
        else
        {
            sale.text = onSalePercentage + "%\nOFF";
        }
        
    }
        

}
