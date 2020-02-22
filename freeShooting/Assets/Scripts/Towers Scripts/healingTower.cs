using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healingTower : MonoBehaviour
{
    public short healing = 25;
    public float fireRate = 1;
    private float nextTimeFire = 0f;
    private GameObject[] target_;

    public healState CurrentState = healState.idle;

    public enum healState
    {
        idle, heal
        //, die
    }

    public void heal(GameObject[] targetGameObject)
    {
        CurrentState = healState.heal;
        target_ = targetGameObject;
    }
    public void stopHeal()
    {
        CurrentState = healState.idle;
        target_ = null;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case healState.idle:
                break;

            case healState.heal:
                if (Time.time > nextTimeFire)
                {
                    foreach (var targetToHeal in target_)
                    {
                        targetToHeal.GetComponent<target>().gainhealth(healing);
                    }
                    nextTimeFire = Time.time + fireRate;            
                }                        
                break;
        }

    }
}
