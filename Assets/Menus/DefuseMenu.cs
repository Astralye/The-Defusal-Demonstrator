using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefuseMenu : Menus
{
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public override void openMenu()
    {
        canvas.enabled = true;
        Time.timeScale = 0;
        PauseMenu.isPaused = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void closeMenu()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
        PauseMenu.isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
