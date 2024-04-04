using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{

    private InputActions inputActions;
    private bool openInventory;
    private bool removeMenu;
    private bool click;
    
    Grid grid;

    [SerializeField]
    private Canvas canvas;



    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(8, 5, 100, new Vector3(100,150,0));
        openInventory = false;
        removeMenu = false;
        click = false;
    }

    // Update is called once per frame
    void Update()
    {
        inputActions.Player.Inventory.started += x => { openInventory = !openInventory; };
        
        inputActions.Menu.Click.started += x => { click = true; };
        inputActions.Menu.Click.canceled += x => { click = false; };

        if (openInventory)
        {
            // Create object if it doesn't exist
            openMenu();
            
            if (click)
            {
                Debug.Log("Clicked");
            }

        }
        else if (removeMenu)
        {
            closeMenu();
        }
    }

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    public void openMenu()
    {
        canvas.enabled = true;
        grid.SetPosition();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        GameObject.Find("PlayerCamera").GetComponent<FirstPersonCamera>().enabled = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;

        removeMenu = true;

        inputActions.Menu.Enable();
    }

    public void closeMenu()
    {
        canvas.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        GameObject.Find("PlayerCamera").GetComponent<FirstPersonCamera>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;

        removeMenu = false;

        inputActions.Player.Enable();
        inputActions.Menu.Disable();
    }
}
