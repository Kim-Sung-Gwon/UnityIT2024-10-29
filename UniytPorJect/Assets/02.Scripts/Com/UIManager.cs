using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager U_Manager;

    public RectTransform gameOverUi;
    public RectTransform GameMenuPanel;
    public RectTransform HpbarPanel;
    public Text PlayTime;
    public Text EndKill;
    public Text Timer;
    public Text KillText;
    public Image Zoom_Image;

    void Awake()
    {
        GameMenuPanel = GameObject.Find("UI_Canvas").transform.GetChild(0).GetComponent<RectTransform>();
        HpbarPanel = GameObject.Find("UI_Canvas").transform.GetChild(2).GetComponent<RectTransform>();
        gameOverUi = GameObject.Find("UI_Canvas").transform.GetChild(3).GetComponent<RectTransform>();

        PlayTime = gameOverUi.transform.GetChild(2).GetChild(0).GetComponent<Text>();
        EndKill = gameOverUi.transform.GetChild(2).GetChild(1).GetComponent<Text>();
        Timer = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(2).GetComponent<Text>();
        KillText = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(3).GetComponent<Text>();

        Zoom_Image = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(4).GetComponent<Image>();
    }
}
