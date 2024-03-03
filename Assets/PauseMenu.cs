using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menus
{
    public GameObject pauseMenu;
    public static bool isPaused;

    [Header("Menus")]
    public SettingsMenu settingsMenu;

    [Header("Keybind")]
    public KeyCode pauseButton = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseButton))
        {
            if (!isPaused)
            {
                openMenu();
            }
            else
            {
                if (settingsMenu.isOpen)
                {
                    Debug.Log("Here");
                    settingsMenu.closeMenu();
                    return;
                }

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                closeMenu();
            }

        }
    }

    public override void openMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void closeMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
