using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_Manager;

    private GameObject player;
    private UIManager uIManager;

    float sec;
    int min;
    int hor;
    public int KillCount;

    public bool isGameOver = false;

    private void Awake()
    {
        if (G_Manager == null)
            G_Manager = this;
        else if (G_Manager != this)
            Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uIManager.KillText.text = string.Format($"<color=#00ff00>KILL Count : </color>" +
            $"<color=#ff0000>{KillCount}</color>");

        // 마우스 커서 관련
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void KillScore()
    {
        ++KillCount;
       uIManager.KillText.text = string.Format($"<color=#00ff00>KILL Count : </color>" +
            $"<color=#ff0000>{KillCount}</color>");
    }

    public void SetTime()
    {
        sec += Time.deltaTime;
        uIManager.Timer.text = string.Format($"{hor:D2} : {min:D2} : {(int)sec:D2}");
        if (sec > 59f)
        {
            sec = 0;
            min++;
            if (min > 59f)
            {
                min = 0;
                hor++;
            }
        }
    }

    public void GameOverUI()
    {
        uIManager.gameOverUi.gameObject.SetActive(true);
        uIManager.GameMenuPanel.gameObject.SetActive(false);
        uIManager.HpbarPanel.gameObject.SetActive(false);
        uIManager.PlayTime.text = "<color=#00ff00>Play Time</color>\n"
            + "<color=#ff0000>" + uIManager.Timer.text + "</color>";
        uIManager.EndKill.text = uIManager.KillText.text;

        Time.timeScale = 1f;
        player.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        GameOverUI();
        Cursor.visible = true;
    }

    private void Update()
    {
        SetTime();
    }
}
