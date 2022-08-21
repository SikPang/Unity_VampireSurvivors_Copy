using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseWindow;
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Level.GetIsLevelUpTime())
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        if (!Level.GetIsLevelUpTime())
        {
            PauseWindow.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void Resume()
    {
        if (!Level.GetIsLevelUpTime())
        {
            PauseWindow.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
}