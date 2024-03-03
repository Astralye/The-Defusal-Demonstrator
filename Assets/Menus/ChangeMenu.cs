using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenu : Menus
{
    public bool isOpen;

    [SerializeField]
    public GameObject selectedMenu;
    public GameObject otherMenu;
    void Start()
    {
        selectedMenu.SetActive(false);
        isOpen = false;
    }

    public override void openMenu()
    {
        otherMenu.SetActive(false);
        selectedMenu.SetActive(true);
        isOpen = true;
    }

    public override void closeMenu()
    {
        otherMenu.SetActive(true);
        selectedMenu.SetActive(false);
        isOpen = false;
    }
}
