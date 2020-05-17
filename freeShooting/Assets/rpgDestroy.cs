using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rpgDestroy : MonoBehaviour
{
    public float timeDestroy = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyEffect());
    }

    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
}
