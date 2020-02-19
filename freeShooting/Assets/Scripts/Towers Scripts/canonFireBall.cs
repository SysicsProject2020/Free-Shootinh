using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canonFireBall : MonoBehaviour
{
    /// <summary>
    /// script to make the canon fire ball distroy ofter exceed her distination
    /// </summary>
    private Vector3 canonPos;
    private float distance;

    public void pos(Vector3 pos,Vector3 targetPos)
    {
        canonPos = pos;
        distance = Vector3.Distance(pos,targetPos);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Invoke("check", 1f);       
    }

    void check()
    {
        float newDistance = Vector3.Distance(transform.position,canonPos);
        if (newDistance > distance)
        {
            Destroy(gameObject);
        }
    }
}
