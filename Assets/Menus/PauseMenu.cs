using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menus
{
    private Canvas canvas;
    public static bool isPaused;

    [Header("Menus")]
    public ChangeMenu menu1;

    [Header("Keybind")]
    public KeyCode pauseButton = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(pauseButton)) return;
        if (!isPaused) openMenu();
        else
        {
             if (menu1.isOpen)
             {
                menu1.closeMenu();
                return;
             }
             closeMenu();
        }
    }

    public override void openMenu()
    {
        canvas.enabled = true;
        Time.timeScale = 0;
        isPaused = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void closeMenu()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
        isPaused = false;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
