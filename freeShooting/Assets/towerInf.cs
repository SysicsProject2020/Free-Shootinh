using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerInf : MonoBehaviour
{
    public short damage;
    // Start is called before the first frame update
    public void SetDamage(short d)
    {
        damage = d;
    }
    public void SetHealth(short h)
    {
        this.GetComponent<target>().Sethealth(h);
    }
}
