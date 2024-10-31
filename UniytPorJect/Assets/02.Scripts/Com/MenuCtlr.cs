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
        F_Cam = Camera.main.GetComponent<FollowCam>();
    }

    public void Pause()
    {
        isPause = !isPause;
        if (!PauseCavers.gameObject.activeSelf)
        {
            PauseMenu.gameObject.SetActive(true);
            SoundMenu.gameObject.SetActive(false);
            F_Cam.CamOneOn();
            Time.timeScale = 0f;

            PauseCavers.gameObject.SetActive(true);
            player.gameObject.SetActive(false);
        }
    }

    public void Sounds(bool isOpen)
    {
        SoundMenu.gameObject.SetActive(isOpen);
        PauseMenu.gameObject.SetActive(!isOpen);
    }

    public void Resume()
    {
        PauseCavers.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
}
