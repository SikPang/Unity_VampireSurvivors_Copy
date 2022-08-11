using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void ToStartScreen()
    {

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
