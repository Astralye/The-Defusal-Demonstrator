using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{

    private InputActions inputActions;
    private bool openInventory;
    private bool removeMenu;
    private bool click;

    private bool onInitialize;
    private bool onFirstHold;

    // Flag for checking if user opened inventory via key
    // vs inventory opened via interactable object.
    private bool selfInventory;
    
    Grid playerGrid;
    Grid externalGrid;

    Grid previousItemGrid;

    List<Item> externItem;
    List<Item> playerInventory;

    List<GameObject> inventorySprite;

    Item overworldItem;

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

    void Start()
    {
        playerGrid = new Grid(8, 5, cellSize, new Vector3(50,50,0),"PlayerGrid");
        playerInventory = playerData.getPlayerInventory();

        openInventory = false;
        removeMenu = false;
        click = false;

        selfInventory = false;
        onInitialize = true;

        defaultItem = new Item();
        ItemHold = defaultItem;
        itemHoldOffset = Vector3.zero;
        Gridoffset = Vector2Int.zero;

        onFirstHold = true;
    }

    private void Update()
    {
        userActions();

        if (openInventory)
        {
            // Create object if it doesn't exist
            initInventory();
            playerClick();
        }
        else if (removeMenu)
        {
            closeMenu();
        }
    }

    private void userActions()
    {
        inputActions.Player.Inventory.started += x => {

            // Get current state.
            if (!openInventory) { selfInventory = true; }

            if (openInventory) { openInventory = false; }
            else { openInventory = true; }
        };

        inputActions.Menu.Click.started += x => { click = true; };
        inputActions.Menu.Click.performed += x => { click = true; };

        inputActions.Menu.Click.canceled += x => { click = false; };
    }

    // Only runs when used from an item
    public void overworldInventory(Item itemDisplayed)
    {
        externItem = new List<Item>();
        externItem.Add(itemDisplayed);

        openInventory = true;
        selfInventory = false;


        overworldItem = itemDisplayed;
    }

    // Initalizers
    // -------------------------------------------------------------------------------------------------------

    // Runs initializers
    private void initInventory()
    {
        // Can only run once
        if (!onInitialize) { return; }
        onInitialize = false;

        inventorySprite = new List<GameObject>();
        playerInventory = playerData.getPlayerInventory();

        openMenu();

        createSprites(playerGrid, playerInventory, "PlayerGrid");

        if (!selfInventory)
        {
            displayDroppedGrid();
            createSprites(externalGrid, externItem, "ExternGrid");
        }

    }

    private void displayDroppedGrid()
    {
        // The position is relative to bottom left corner of the screen.
        // Would be ideal if the vector was in reference to right side.
        externalGrid = new Grid(4, 5, cellSize, new Vector3(750, 50, 0),"ExternGrid");
        externalGrid.SetPosition();

        // If i randomize HERE, then every time the menu is open, it would keep randomizing,
        // It should only be random once.
        foreach (Item item in externItem)
        {
            Vector2 dimensions = item.getItemDimensions();


            // Checks if item dimensions fit in the external inventory slot dimensions.
            if ((item.inventoryPosition.x + dimensions.x <= externalGrid.getWidth())
                && (item.inventoryPosition.y + dimensions.y <= externalGrid.getHeight()))
            {
                fillInventorySpace(item, externalGrid);
            }
        }
    }

    // Creates sprites in external inventory
    private void createSprites(Grid grid, List<Item> itemList,string parent)
    {

        foreach (Item item in itemList)
        {

            Vector2 dimensions = item.getItemDimensions();

            GameObject itemObject = new GameObject();
            itemObject.name = item.getItemType().ToString();

            itemObject.transform.SetParent(GameObject.Find(parent).transform);

            itemObject.AddComponent<UnityEngine.UI.Image>();

            UnityEngine.UI.Image imageObject = itemObject.GetComponent<UnityEngine.UI.Image>();

            imageObject.rectTransform.anchorMin = Vector2.zero;
            imageObject.rectTransform.anchorMax = Vector2.zero;
            imageObject.rectTransform.pivot = Vector2.zero;

            imageObject.rectTransform.position = grid.GetWorldPosition(item.inventoryPosition);
            imageObject.rectTransform.sizeDelta = new Vector2(dimensions.x * cellSize, dimensions.y * cellSize);
            imageObject.overrideSprite = item.getSpriteTexture();

            itemObject.AddComponent<ImageSpriteID>();
            itemObject.GetComponent<ImageSpriteID>().setID(item.itemID);

            inventorySprite.Add(itemObject);
        }
    }

    // -------------------------------------------------------------------------------------------------------


    // Inventory functions
    // -------------------------------------------------------------------------------------------------------
    
    // Takes in an item and the corresponding grid.
    private void fillInventorySpace(Item item, Grid grid)
    {

        Vector2 dimensions = item.getItemDimensions();

        // 4 x 5
        for (int x = 0; x < dimensions.x; x++){
            for (int y = 0; y < dimensions.y; y++){
                grid.gridArray[x + item.inventoryPosition.x,
                               y + item.inventoryPosition.y] = item;
            }
        }
    }

    private UnityEngine.UI.Image getSpriteFromID(int ID)
    {
        // For each item
        foreach (GameObject sprite in inventorySprite)
        {
            if (sprite.GetComponent<ImageSpriteID>().getID() == ID) { return sprite.GetComponent<UnityEngine.UI.Image>(); }
        }

        return null;
    }

    private void destroySprites()
    {
        // For each item
        foreach (GameObject sprite in inventorySprite)
        {
            Destroy(sprite);
        }
    }

    private void holdingItem()
    {
        if (ItemHold == defaultItem) { return; }

        UnityEngine.UI.Image itemObject = getSpriteFromID(ItemHold.itemID);
        itemObject.transform.position = mousePosition() - itemHoldOffset;
    }

    // Function for moving inventory items around
    private void playerClick()
    {
        // Done before click so drag and drop can work.
        Vector2 mousePos = mousePosition();

        Grid mouseHoverGrid = gridSelector();

        Vector2Int gridCoord;
        bool nullGridValue;
        bool validItem;

        holdingItem();

        // Checks if mouse is in valid position
        try
        {
            gridCoord = mouseHoverGrid.getXY(mousePos);
            nullGridValue = false;

            validItem = validSlot(gridCoord, mouseHoverGrid);
        }
        catch (Exception e) // null exception
        {
            gridCoord = Vector2Int.zero;
            nullGridValue = true;
            validItem = false;
        }

        // On hold, runs only once
        if (click && validItem && onFirstHold)
        {
            // Get item clicked
            ItemHold = mouseHoverGrid.gridArray[gridCoord.x, gridCoord.y];

            // Get the offset of click location to change base position
            itemHoldOffset = transform.position - mouseHoverGrid.GetWorldPosition(ItemHold.inventoryPosition);
            Gridoffset = ItemHold.inventoryPosition - mouseHoverGrid.getXY(mousePos);

            // Flags
            onFirstHold = false;

            previousItemGrid = mouseHoverGrid;
            oldLocation = ItemHold.inventoryPosition;
        }
        // On drop
        else if (!click && !onFirstHold)
        {
            try
            {
                // Check if dropped at valid location or null grid
                if(nullGridValue || !(checkValidLocation(ItemHold, mouseHoverGrid.getXY(mousePos) + Gridoffset, mouseHoverGrid)))
                { throw new Exception(); }

                // Update position
                updateMoveInventory(ItemHold, mouseHoverGrid.getXY(mousePos) + Gridoffset, mouseHoverGrid);

                // If the item is placed in a different grid, remove it from the list.
                if(previousItemGrid != mouseHoverGrid) { changeLocationItem(ItemHold, mouseHoverGrid); }
            }
            catch(Exception e) // Code only runs if item is dropped in non grid area.
            {
                updateMoveInventory(ItemHold, oldLocation, previousItemGrid);
            }
            finally
            {
                // Reset Item hold
                ItemHold = defaultItem;
                itemHoldOffset = Vector3.zero;

                // Flags
                onFirstHold = true;
            }


        }
    }

    private void changeLocationItem(Item item, Grid newGrid)
    {
        if (newGrid == playerGrid)
        {
            playerInventory.Add(item);
            externItem.Remove(item);
            GameObject.Find(item.getItemType().ToString()).transform.SetParent(GameObject.Find("PlayerGrid").transform);
        }
        else
        {
            playerInventory.Remove(item);
            externItem.Add(item);
            GameObject.Find(item.getItemType().ToString()).transform.SetParent(GameObject.Find("ExternGrid").transform);
        }
    }

    // Determines which grid the cursor is over
    private Grid gridSelector()
    {
        if (selfInventory) { return playerGrid; }

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

    // Gets mouse position in UI coordinates
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

    // Updates location of dropped item
    private void updateMoveInventory(Item item, Vector2Int newLocation, Grid grid)
    {
        Vector2 dimensions = item.getItemDimensions();


        // Remove everything from the old location
        for (int x = 0; x < dimensions.x; x++){
            for (int y = 0; y < dimensions.y; y++){
                previousItemGrid.gridArray[oldLocation.x + x, oldLocation.y + y] = null;
            }
        }

        // Sets the new location
        item.inventoryPosition = newLocation;

        //// Put everything in the new location
        for (int x = 0; x < dimensions.x; x++){
            for (int y = 0; y < dimensions.y; y++){
                Vector2Int position = item.inventoryPosition + new Vector2Int(x, y);
                grid.gridArray[position.x, position.y] = item;
            }
        }

        // Snaps item to location
        UnityEngine.UI.Image itemObject = getSpriteFromID(item.itemID);
        itemObject.rectTransform.position = grid.GetWorldPosition(ItemHold.inventoryPosition);
    }

    private bool checkValidLocation(Item item, Vector2Int newLocation, Grid grid)
    {
        Vector2 dimensions = item.getItemDimensions();


        // Check what grid it is using 
        for (int x = 0; x < dimensions.x; x++){
            for (int y = 0; y < dimensions.y; y++) {

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

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    // Reset inventory states
    // -------------------------------------------------------------------------------------------------------

    private void resetItemFlag()
    {
        if (!selfInventory)
        {
            foreach (Item item in externItem)
            {
                item.resetRunOnce();
            }
        }

        foreach (Item item in playerInventory)
        {
            item.resetRunOnce();
        }

    }

    // Set and reset flags for opening and closing the menu

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

        resetItemFlag();
        destroySprites();

        playerData.setInventory(playerInventory);

        if (!selfInventory)
        {
            if (externItem.Count == 0) { overworldItem.destroyItem(); }
            externalGrid.closeInventory();
        }
    }
}
