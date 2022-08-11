using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseWindow;
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        PauseWindow.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        PauseWindow.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}