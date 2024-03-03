using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : Menus
{
    public bool isOpen;

    [SerializeField]
    public GameObject settingsMenu;
    public GameObject otherMenu;
    void Start()
    {
        settingsMenu.SetActive(false);
        isOpen = false;
    }

    public override void openMenu()
    {
        otherMenu.SetActive(false);
        settingsMenu.SetActive(true);
        isOpen = true;
    }

    public override void closeMenu()
    {
        otherMenu.SetActive(true);
        settingsMenu.SetActive(false);
        isOpen = false;
    }
}
