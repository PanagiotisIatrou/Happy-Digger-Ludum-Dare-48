using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Singleton
    private static PauseManager _instance;
    public static PauseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PauseManager>();
            }

            return _instance;
        }

    }

    public GameObject PausePanelGO;
    private bool isPaused = false;

    public static bool IsPaused()
    {
        return Instance.isPaused;
    }

    public static void Pause()
    {
        Instance.isPaused = true;
        AudioListener.pause = true;
        Instance.PausePanelGO.SetActive(true);
        Time.timeScale = 0f;
    }

    public static void UnPause()
    {
        Instance.isPaused = false;
        AudioListener.pause = false;
        Instance.PausePanelGO.SetActive(false);
        Time.timeScale = 1f;
    }
}
