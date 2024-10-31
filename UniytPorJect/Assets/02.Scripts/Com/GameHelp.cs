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

        if (!HelpCanves.gameObject.activeSelf)
        {
            LoadMainPanel();
            HelpCanves.gameObject.SetActive(true);
        }
    }

    public void Nextpanel(bool isOpen)
    {
        NextPanel.gameObject.SetActive(isOpen);
        MainPanel.gameObject.SetActive(!isOpen);
    }

    public void Beforpanel(bool isOpen)
    {
         NextPanel.gameObject.SetActive(!isOpen);
         BeforePanel.gameObject.SetActive(isOpen);
    }

    public void Resume()
    {
        LoadMainPanel();
        HelpCanves.gameObject.SetActive(false);
    }

    private void LoadMainPanel()
    {
        MainPanel.gameObject.SetActive(true);
        NextPanel.gameObject.SetActive(false);
        BeforePanel.gameObject.SetActive(false);
    }
}
