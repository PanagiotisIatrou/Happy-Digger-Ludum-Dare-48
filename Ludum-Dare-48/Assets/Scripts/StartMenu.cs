using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private bool pressed = false;

    private void Update()
    {
        if (pressed)
            return;

        if (Input.anyKey)
        {
            pressed = true;
            GameManager.StartGame();
        }
    }
}
