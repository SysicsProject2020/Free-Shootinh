using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    Text txt;
    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
    }

    
    // Update is called once per frame
    void Update()
    {
        float a = Mathf.Round(1 / Time.deltaTime);
        txt.text = a.ToString();
    }
}
