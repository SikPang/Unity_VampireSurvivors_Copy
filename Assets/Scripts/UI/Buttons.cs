using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ToStartScreen()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }

    public void ToGameScreen()
    {
        SceneManager.LoadScene("InGame");
    }

    public void Pause()
    {
        PauseMenu.isPaused = true;
    }

    public void Resume()
    {
        PauseMenu.isPaused = false;
    }
}
