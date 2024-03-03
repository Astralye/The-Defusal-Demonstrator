using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menus
{
    public GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
    }

    public override void openMenu()
    {
        Time.timeScale = 1;
    }

    public override void closeMenu()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
