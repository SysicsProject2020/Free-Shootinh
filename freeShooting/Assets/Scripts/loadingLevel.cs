using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class loadingLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject loadingScreen;
    public Slider slider;
    public TMPro.TextMeshProUGUI ProgressTxt;
    string[] loading = { "LOADING", "LOADING.", "LOADING..", "LOADING..." };
    public GameObject hero;

    public void LoadLevel(int sceneIndex)
    {
        if (sceneIndex == 1)
        {
            hero.SetActive(false);
        }
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        float timer = 0f;
        float minLoadTime = 1.5f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        loadingScreen.SetActive(true);
        StartCoroutine(Load());
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            //Debug.Log(progress);

            timer += Time.deltaTime;

            if (timer > minLoadTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    IEnumerator Load()
    {
        int i = -1;
        while (true)
        {
            i++;
            if (i ==4)
            {
                i = 0;
            }
            ProgressTxt.text = loading[i];
            yield return new WaitForSeconds(0.5f);
        }

    }
}
