using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameStartAndQuit : MonoBehaviour
{
    public void OnChlickPlay()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickRePlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.G_Manager.isGameOver = false;
    }

    public void OnChlickQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
