using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Assertions.Must;

public class Inventory : MonoBehaviour
{

    private InputActions inputActions;
    private bool openInventory;
    private bool removeMenu;
    private bool click;

    private bool onInitialize;
    private bool onFirstHold;
    private bool holdItem;
    private bool snapToCell;

    // Flag for checking if user opened inventory via key
    // vs inventory opened via interactable object.
    private bool selfInventory;

    private bool initExternGrid;
    
    Grid playerGrid;
    Grid externalGrid;

    Grid previousItemGrid;

    List<Item> externItem = new List<Item>();
    List<Item> playerInventory;

    Item ItemHold;
    Item defaultItem;

    Vector3 itemHoldOffset;
    Vector2Int Gridoffset;
    Vector2Int oldLocation;

    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private int cellSize;


    // Start is called before the first frame update
    void Start()
    {
        playerGrid = new Grid(8, 5, cellSize, new Vector3(50,50,0),"PlayerGrid");
        playerInventory = playerData.getPlayerInventory();

        openInventory = false;
        removeMenu = false;
        click = false;

        selfInventory = false;
        initExternGrid = false;

        onInitialize = true;

        defaultItem = new Item();
        ItemHold = defaultItem;
        itemHoldOffset = Vector3.zero;
        Gridoffset = Vector2Int.zero;

        onFirstHold = true;
        holdItem = false;
        snapToCell = false;
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
        inputActions.Menu.Click.performed += x => { click = true; };

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
                               y + item.inventoryPosition.y] = item;
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
                displayItems(externalGrid, externItem);
            }
            else
            {
                // Have a slightly different GUI different 
            }

            displayItems(playerGrid, playerInventory);
            playerClick();
        }
        else if (removeMenu)
        {
            closeMenu();
        }
    }

    private void displayItems(Grid grid, List<Item> itemList)
    {
        foreach(Item item in itemList)
        {
            if (onInitialize && !selfInventory)
            {
                GameObject itemObject = new GameObject();
                itemObject.name = item.getItemType().ToString();

                itemObject.transform.SetParent(GameObject.Find("ExternGrid").transform);

                itemObject.AddComponent<UnityEngine.UI.Image>();
                UnityEngine.UI.Image imageObject = itemObject.GetComponent<UnityEngine.UI.Image>();

                imageObject.rectTransform.anchorMin = Vector2.zero;
                imageObject.rectTransform.anchorMax = Vector2.zero;
                imageObject.rectTransform.pivot = Vector2.zero;

                imageObject.rectTransform.position = grid.GetWorldPosition(item.inventoryPosition);
                imageObject.rectTransform.sizeDelta = new Vector2(item.Dimensions.x * cellSize, item.Dimensions.y * cellSize);
                imageObject.overrideSprite = item.getSpriteTexture();
            }
            else
            {
                UnityEngine.UI.Image imageObject = GameObject.Find("Pistol").GetComponent<UnityEngine.UI.Image>();

                // Position of the mouse
                Vector3 mousePos = mousePosition();

                if (holdItem)
                {
                    imageObject.transform.position = mousePos - itemHoldOffset;
                }

                if (snapToCell)
                {
                    imageObject.rectTransform.position = grid.GetWorldPosition(item.inventoryPosition);
                    snapToCell = false;
                }
            }

        }

        onInitialize = false;
    }

    private void playerClick()
    {
        // Done before click so drag and drop can work.
        Vector2 mousePos = mousePosition();

        Grid mouseHoverGrid = gridSelector();
        if (mouseHoverGrid == null) { return; }

        // Player inventory
        Vector2Int gridCoord = playerGrid.getXY(mousePos);

        // External Inventory
        if (!selfInventory)
        {
            gridCoord = mouseHoverGrid.getXY(mousePos);
            // Within the bounds of the grid and clicked on a non null tile
            if (!mouseHoverGrid.validPosition(gridCoord)) return;
        }

        bool validItem = validSlot(gridCoord, mouseHoverGrid);

        // On hold
        if (click && validItem && onFirstHold)
        {
            // Get item clicked
            ItemHold = getItemClicked(gridCoord, mouseHoverGrid);

            // Get the offset of click location to change base position
            itemHoldOffset = transform.position - mouseHoverGrid.GetWorldPosition(ItemHold.inventoryPosition);
            Gridoffset = ItemHold.inventoryPosition - mouseHoverGrid.getXY(mousePos);

            // Flags
            onFirstHold = false;
            holdItem = true;

            previousItemGrid = mouseHoverGrid;
            oldLocation = ItemHold.inventoryPosition;
        }
        // On drop
        else if (!click && !onFirstHold)
        {
            // Check if dropped at valid location
            if (!checkValidLocation(ItemHold, mouseHoverGrid.getXY(mousePos) + Gridoffset, mouseHoverGrid)) { return; }

            // Update position
            updateMoveInventory(ItemHold, mouseHoverGrid.getXY(mousePos) + Gridoffset, mouseHoverGrid);

            // If the item is placed in a different grid, remove it from the list.
            if(previousItemGrid != mouseHoverGrid) { changeLocationItem(ItemHold); }

            // Reset Item hold
            ItemHold = defaultItem;
            itemHoldOffset = Vector3.zero;

            // Flags
            onFirstHold = true;
            holdItem = false;
            snapToCell = true;
        }
    }

    private void changeLocationItem(Item item)
    {
        List<Item> oldPos;
        List<Item> newPos;

        Grid grid = gridSelector();

        if (grid == playerGrid)
        {
            playerInventory.Add(item);
            externItem.Remove(item);
        }
        else
        {
            playerInventory.Remove(item);
            externItem.Add(item);
        }
    }

        // Determines which grid the cursor is over
    private Grid gridSelector()
    {
        Vector3 mousePos = mousePosition();

        Vector2 min = playerGrid.getOrigin();
        Vector2 max = playerGrid.getGridArea();

        if(mousePos.x >= min.x && mousePos.x <= max.x &&
           mousePos.y >= min.y && mousePos.y <= max.y)
        {
            return playerGrid;
        }

        min = externalGrid.getOrigin();
        max = externalGrid.getGridArea();

        if (mousePos.x >= min.x && mousePos.x <= max.x &&
            mousePos.y >= min.y && mousePos.y <= max.y)
        {
            return externalGrid;
        }
        return null;
    }

    private Vector3 mousePosition()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera,
            out movePos);

        transform.position = canvas.transform.TransformPoint(movePos);

        return transform.position;
    }

    private void updateMoveInventory(Item item, Vector2Int newLocation, Grid grid)
    {

        // Remove everything from the old location
        for (int x = 0; x < item.Dimensions.x; x++){
            for (int y = 0; y < item.Dimensions.y; y++){
                previousItemGrid.gridArray[oldLocation.x + x, oldLocation.y + y] = null;
            }
        }

        item.inventoryPosition = newLocation;

        //// Put everything in the new location
        for (int x = 0; x < item.Dimensions.x; x++){
            for (int y = 0; y < item.Dimensions.y; y++){
                Vector2Int position = item.inventoryPosition + new Vector2Int(x, y);
                grid.gridArray[position.x, position.y] = item;
            }
        }
    }

    private bool checkValidLocation(Item item, Vector2Int newLocation, Grid grid)
    {
        for (int x = 0; x < item.Dimensions.x; x++){
            for (int y = 0; y < item.Dimensions.y; y++) {

                Vector2Int position = newLocation + new Vector2Int(x, y);
                if (!validSlot(position, grid)) { continue; }

                Item itemGrid = grid.gridArray[position.x, position.y];

                // If they are not an empty slot or the same ID, it cannot move to the location
                if (!(itemGrid.itemID == item.itemID && grid.validPosition(position))) { return false; }

            }
        }

        return true;
    }

    // Throws an exception if null value (empty location)
    private bool validSlot(Vector2Int position, Grid grid)
    {
        //return (grid.gridArray[position.x, position.y].itemID != 0) ? true : false;
        try
        {
            if (grid.gridArray[position.x, position.y].itemID != 0) { }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private Item getItemClicked(Vector2Int position, Grid grid)
    {
        return grid.gridArray[position.x, position.y];
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


        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;

        GameObject.Find("PlayerCamera").GetComponent<FirstPersonCamera>().enabled = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;

        removeMenu = true;

        inputActions.Menu.Enable();
    }

    public void closeMenu()
    {
        canvas.enabled = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;


        GameObject.Find("PlayerCamera").GetComponent<FirstPersonCamera>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;

        removeMenu = false;

        inputActions.Player.Enable();
        inputActions.Menu.Disable();

        onInitialize = true;
    }
}
