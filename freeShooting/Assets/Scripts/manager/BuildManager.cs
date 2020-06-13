using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuildManager : MonoBehaviour
{  
    public Text startCoinsTxt;
    private TowerScript[] towers= new TowerScript[6];

    bool notAlreadyClicked = false;
    public short num;
    public short waitBetweenBuild = 7;
    public short selectTime = 5;
    Coroutine lastRoutine = null;

    // Start is called before the first frame update
    void Start()
    {
        towers = GetComponent<GameManager>().GetSelectedTowers();
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetMouseButtonDown(0) && notAlreadyClicked)
        {
            //StopCoroutine(lastRoutine);
            testBuilding();
        }
    }

    public void click (int nb)
    {
        if (lastRoutine != null)
        {
            GameManagerPartie.instance.itemParent.transform.GetChild(num).GetChild(0).gameObject.SetActive(false);
            GameManagerPartie.instance.itemParent.transform.GetChild(num).GetChild(0).GetComponent<Image>().fillAmount = 0;
            GameManagerPartie.instance.itemParent.transform.GetChild(num).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 0;
            StopCoroutine(lastRoutine);
        }        
        notAlreadyClicked = true;       
        num = (short)nb;
        //change sprite to selected

        lastRoutine = StartCoroutine(selected(num));

    }
    public void unclick()
    {
        //StopCoroutine("selected");
        

        notAlreadyClicked = false;
        
    }

    IEnumerator loading(Image circleImg,TextMeshProUGUI txt, short nb)
    {
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(1).GetComponent<Image>().sprite = GameManagerPartie.instance.towersSelected[nb].Lockedimage;
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).gameObject.SetActive(false);
        

        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetComponent<Button>().interactable = false;
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(3).gameObject.SetActive(true);
        short t = waitBetweenBuild;
        t *= 25;//fps
        for (int i = 0; i < t; i++)
        {
            yield return new WaitForSeconds(1f / 25f);
            float time = waitBetweenBuild - Mathf.Round((float)i / 25);
            txt.text = time.ToString() + "s";
            circleImg.fillAmount = (float)i / (float)t;
        }
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(3).gameObject.SetActive(false);

        if (GameManagerPartie.instance.towersSelected[nb].cost <= GameManagerPartie.instance.playerCoins)
        {
            GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetComponent<Button>().interactable = true;
            GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(1).GetComponent<Image>().sprite = GameManagerPartie.instance.towersSelected[nb].image;
        }
        

    }

    IEnumerator selected(short nb)
    {
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).gameObject.SetActive(true);
        for (int j = 0; j < selectTime; j++)
        {
            for (int i = 0; i < 25; i++)
            {
                yield return new WaitForSeconds(1f / 25f);
                float fill = (float)i / 25 / 2;
                GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).GetComponent<Image>().fillAmount = fill;
                GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = fill;
            }
            for (int i = 0; i < 25; i++)
            {
                yield return new WaitForSeconds(1f / 25f);
                float fill = 0.5f - ((float)i / 25 / 2);
                GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).GetComponent<Image>().fillAmount = fill;
                GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = fill;
            }
        }
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).gameObject.SetActive(false);
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).GetComponent<Image>().fillAmount = 0;
        GameManagerPartie.instance.itemParent.transform.GetChild(nb).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 0;
    }


    public void testBuilding()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (lastRoutine != null)
            {
                GameManagerPartie.instance.itemParent.transform.GetChild(num).GetChild(0).gameObject.SetActive(false);
                GameManagerPartie.instance.itemParent.transform.GetChild(num).GetChild(0).GetComponent<Image>().fillAmount = 0;
                GameManagerPartie.instance.itemParent.transform.GetChild(num).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 0;
                StopCoroutine(lastRoutine);
            }
            if (hit.collider.tag == "TowerDefendZone")
            {
                Vector3 towerpos = new Vector3(hit.collider.transform.position.x, transform.position.y, hit.collider.transform.position.z);
                positionManager.add(towers[num], towerpos);

                Image circleImg = GameManagerPartie.instance.itemParent.transform.GetChild(num).GetChild(3).GetChild(0).GetComponent<Image>();
                TextMeshProUGUI txt = GameManagerPartie.instance.itemParent.transform.GetChild(num).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
                StartCoroutine(loading(circleImg, txt, num));
            }
            unclick();
        }
    }
}
