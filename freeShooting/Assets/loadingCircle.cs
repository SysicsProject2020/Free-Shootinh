using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingCircle : MonoBehaviour
{
    public Image circleImg;
    public TMPro.TextMeshProUGUI txt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("loading");
    }


    IEnumerator loading()
    {
        short t = BuildManager.waitBetweenBuild;
        t *= 25;//fps
        Debug.Log(t);
        for (int i = 0; i < t; i++)
        {
            yield return new WaitForSeconds(1f / 25f);
            float time = BuildManager.waitBetweenBuild - Mathf.Round((float)i / 25);
            txt.text = time.ToString() + "s";
            circleImg.fillAmount = (float)i / (float)t;          
        }
    }
}
