using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelp : MonoBehaviour
{
    private RectTransform HelpCanves;
    private RectTransform MainPanel;
    private RectTransform NextPanel;
    private RectTransform BeforePanel;

    bool isPause;

    void Start()
    {
        HelpCanves = GameObject.Find("Canvas").transform.GetChild(4).GetComponent<RectTransform>();
        MainPanel = HelpCanves.transform.GetChild(0).GetComponent<RectTransform>();
        NextPanel = HelpCanves.transform.GetChild(1).GetComponent<RectTransform>();
        BeforePanel = HelpCanves.transform.GetChild(2).GetComponent<RectTransform>();
    }

    public void HelpPause()
    {
        isPause = !isPause;

        if (HelpCanves.gameObject.activeSelf == false)
        {
            if (!HelpCanves.gameObject.activeInHierarchy)
            {
                MainPanel.gameObject.SetActive(true);
                NextPanel.gameObject.SetActive(false);
            }
            HelpCanves.gameObject.SetActive(true);
        }
    }

    public void Nextpanel(bool isOpen)
    {
        if (isOpen)
        {
            NextPanel.gameObject.SetActive(isOpen);
            MainPanel.gameObject.SetActive(false);
        }
        else
        {
            NextPanel.gameObject.SetActive(false);
            MainPanel.gameObject.SetActive(true);
        }
    }

    public void Beforpanel(bool isOpen)
    {
        if (isOpen)
        {
            NextPanel.gameObject.SetActive(false);
            BeforePanel.gameObject.SetActive(isOpen);
        }
        else
        {
            NextPanel.gameObject.SetActive(true);
            BeforePanel.gameObject.SetActive(false);
        }
    }

    public void Resume()
    {
        MainPanel.gameObject.SetActive(true);
        NextPanel.gameObject.SetActive(false);
        BeforePanel.gameObject.SetActive(false);
        HelpCanves.gameObject.SetActive(false);
    }
}
