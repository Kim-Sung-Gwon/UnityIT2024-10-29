using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCtlr : MonoBehaviour
{
    public static MenuCtlr M_Ctrl;

    public RectTransform PauseCavers;
    public RectTransform PauseMenu;
    public RectTransform SoundMenu;
    public GameObject player;

    private FollowCam F_Cam;

    bool isPause;

    void Awake()
    {
        isPause = false;
        player = GameObject.FindGameObjectWithTag("Player");
        PauseCavers = GameObject.Find("UI_Canvas").transform.GetChild(1).GetComponent<RectTransform>();
        PauseMenu = PauseCavers.GetChild(0).GetComponent<RectTransform>();
        SoundMenu = PauseCavers.GetChild(1).GetComponent<RectTransform>();
    }

    private void Start()
    {
        F_Cam = GameObject.Find("Main Camera").GetComponent<FollowCam>();
    }

    public void Pause()
    {
        isPause = !isPause;
        if (PauseCavers.gameObject.activeSelf == false)
        {
            if (!PauseMenu.gameObject.activeInHierarchy)
            {
                PauseMenu.gameObject.SetActive(true);
                SoundMenu.gameObject.SetActive(false);
            }
            PauseCavers.gameObject.SetActive(true);
            player.gameObject.SetActive(false);
            F_Cam.CamOneOn();
            Time.timeScale = 0f;
        }
    }

    public void Sounds(bool isOpen)
    {
        if (isOpen)
        {
            SoundMenu.gameObject.SetActive(isOpen);
            PauseMenu.gameObject.SetActive(false);
        }
        else
        {
            SoundMenu.gameObject.SetActive(false);
            PauseMenu.gameObject.SetActive(true);
        }
    }

    public void Resume()
    {
        SoundMenu.gameObject.SetActive(false);
        PauseCavers.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
}
