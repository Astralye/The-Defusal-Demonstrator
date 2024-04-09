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



    // Flag for checking if user opened inventory via key
    // vs inventory opened via interactable object.
    private bool selfInventory;

    private bool initExternGrid;
    
    Grid playerGrid;
    Grid externalGrid;

    List<Item> externItem = new List<Item>();



    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private int cellSize;


    // Start is called before the first frame update
    void Start()
    {
        playerGrid = new Grid(8, 5, cellSize, new Vector3(50,50,0),"PlayerGrid");
        openInventory = false;
        removeMenu = false;
        click = false;

        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera,
            out pos);

        selfInventory = false;
        initExternGrid = false;
    }

    public void overworldInventory(Item itemDisplayed)
    {
        openInventory = true;
        selfInventory = false;
        initExternGrid = true;

        externItem.Add(itemDisplayed);
    }

    private void userActions()
    {

        inputActions.Player.Inventory.started += x => {
            openInventory = !openInventory;
            selfInventory = true;
        };

        inputActions.Menu.Click.started += x => { click = true; };
        inputActions.Menu.Click.canceled += x => { click = false; };
    }

    private void displayDroppedGrid()
    {

        // Initialize grid
        if (initExternGrid)
        {
            // The position is relative to bottom left corner of the screen.
            // Would be ideal if the vector was in reference to right side.
            externalGrid = new Grid(4, 5, cellSize, new Vector3(750, 50, 0),"ExternGrid");
            initExternGrid = false;
        }

        externalGrid.SetPosition();

        // Display all the items in the grid.

        // If i randomize HERE, then every time the menu is open, it would keep randomizing,
        // It should only be random once.
        foreach (Item item in externItem)
        {
            
            // Checks if item dimensions fit in the external inventory slot dimensions.
            if ((item.inventoryPosition.x + item.Dimensions.x <= externalGrid.getWidth())
                && (item.inventoryPosition.y + item.Dimensions.y <= externalGrid.getHeight()))
            {
                // Fill the inventory space with the item value.
                fillInventorySpace(item, externalGrid);
            }
        }
    }

    // Takes in an item and the corresponding grid.
    private void fillInventorySpace(Item item, Grid grid)
    {
        // 4 x 5
        for (int x = 0; x < item.Dimensions.x; x++)
        {
            for (int y = 0; y < item.Dimensions.y; y++)
            {
                grid.gridArray[x + item.inventoryPosition.x,
                               y + item.inventoryPosition.y] = item.getItemType();
            }
        }
    }

    private void Update()
    {
        userActions();

        if (openInventory)
        {
            // Create object if it doesn't exist
            openMenu();
            
            // Player pick up item.
            if (!selfInventory)
            {
                displayDroppedGrid();
                displayItems(externalGrid);
            }
            else
            {
                // Have a slightly different GUI different 
            }
            
            displayItems(playerGrid);

            playerClick();

            // Display items
        }
        else if (removeMenu)
        {
            closeMenu();
        }
    }

    private void displayItems(Grid grid)
    {
        // TODO NEXT TIME

        // By doing it by item, we can use varied sized tiles, e.g 2x3 for a texture.
        // If doing this via type, we cannot determine where the texture tile starts or ends.

        // 1: Loop through all the items in the grid
        // 2: Get the position of valid item boxes.
        // 3: Convert item coordinates to world coordinates
        // 4: Using the cellwidth, fill the area with them a unique colour / image.
        // 5: Do this for every valid 

        // 4.5? Need to know how to split up an image over multiple dimensions?
        // E.g 2x3, Each tile has a unique texture. Would need the orientation.
    }

    private void playerClick()
    {
        // Done before click so drag and drop can work.
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera,
            out movePos);

        transform.position = canvas.transform.TransformPoint(movePos);

        // Clicks on inventory grid
        if (click)
        {
            // Player inventory
            Vector2Int gridCoord = playerGrid.getXY(transform.position);
            if (playerGrid.validPosition(gridCoord))
            {
                if (ClickItem(gridCoord, playerGrid))
                {

                }
            }
            
            
            if (!selfInventory)
            {
                // Extern inventory
                gridCoord = externalGrid.getXY(transform.position);

                if (externalGrid.validPosition(gridCoord))
                {
                    if (ClickItem(gridCoord, externalGrid))
                    {
                    }
                }

            }
        }
    }

    private bool ClickItem(Vector2Int position, Grid grid)
    {
        if (grid.gridArray[position.x, position.y] != ItemList.Items.None) return true;
        return false;
    }

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    public void openMenu()
    {
        canvas.enabled = true;
        playerGrid.SetPosition();

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
