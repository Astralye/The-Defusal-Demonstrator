using System;
using System.Collections.Generic;
using UnityEngine;

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

    private bool firstInstance;

    // Flag for checking if user opened inventory via key
    // vs inventory opened via interactable object.
    private bool selfInventory;

    private bool initExternGrid;
    
    Grid playerGrid;
    Grid externalGrid;

    Grid previousItemGrid;

    List<Item> externItem = new List<Item>();
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

    public void overworldInventory(Item itemDisplayed, bool firstInstance)
    {
        openInventory = true;
        selfInventory = false;
        initExternGrid = true;

        this.firstInstance = firstInstance;

        if (firstInstance) {
            externItem.Add(itemDisplayed);
        }

        overworldItem = itemDisplayed;
    }

    private void resetItemFlag()
    {
        if (!selfInventory){
            foreach(Item item in externItem){
                item.resetRunOnce();
            }
        }

        foreach (Item item in playerInventory)
        {
            item.resetRunOnce();
        }

    }

    private void userActions()
    {
        inputActions.Player.Inventory.started += x => {

            // Get current state.
            if (!openInventory) { selfInventory = true; }

            if (openInventory) { openInventory = false; }
            else{ openInventory = true; }
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
                externalGrid = null;
            }

            displayItems(playerGrid, playerInventory);
            playerClick();
        }
        else if (removeMenu)
        {
            closeMenu();
        }
    }

    private void destroyExternalInventoryImages()
    {
        GameObject externalInventoryObjects = GameObject.Find("ExternGrid");

        for (int i = 0; i < externalInventoryObjects.transform.childCount; i++)
        {
            Destroy(externalInventoryObjects.transform.GetChild(i).gameObject);
        }
    }

    private bool checkEmptyGrid()
    {
        return (externItem.Count == 0) ? true : false;
    }

    private Item getItemFromID(List<Item> itemList, int ID)
    {
        // For each item
        foreach (Item item in itemList)
        {
            if (item.itemID == ID) { return item; }
        }

        return null;
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

    // Create function, input is Item item, output is the image object.
    // both the object and the item can use an ITEM ID to make sure they have the same value.
    private void displayItems(Grid grid, List<Item> itemList)
    {
        if (onInitialize && !selfInventory)
        {
            // Create new game objects only if it is not self inventory.
            inventorySprite = new List<GameObject>();
            onInitialize = false;

            foreach (Item item in itemList)
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

                itemObject.AddComponent<ImageSpriteID>();
                itemObject.GetComponent<ImageSpriteID>().setID(item.itemID);

                inventorySprite.Add(itemObject);
            }
        }

        // All the sprites to render.
        foreach (Item item in itemList)
        {
            UnityEngine.UI.Image itemObject = getSpriteFromID(item.itemID);

            // Position of the mouse
            Vector3 mousePos = mousePosition();

            if (holdItem)
            {
                itemObject.transform.position = mousePos - itemHoldOffset;
            }
            else if (snapToCell)
            {
                itemObject.rectTransform.position = grid.GetWorldPosition(item.inventoryPosition);
                snapToCell = false;
            }
        }


        //// All the sprites to render.
        //foreach (GameObject sprite in inventorySprite)
        //{
        //    UnityEngine.UI.Image itemObject = sprite.GetComponent<UnityEngine.UI.Image>();

        //    // Position of the mouse
        //    Vector3 mousePos = mousePosition();

        //    if (holdItem){
        //        itemObject.transform.position = mousePos - itemHoldOffset;
        //    }
        //    else if (snapToCell)
        //    {
        //        Item item = getItemFromID(itemList, itemObject.GetComponent<ImageSpriteID>().getID());

        //        if (item != null)
        //        {
        //            itemObject.rectTransform.position = grid.GetWorldPosition(item.inventoryPosition);
        //            snapToCell = false;
        //        }
        //    }
        //}
    }

    private void playerClick()
    {
        // Done before click so drag and drop can work.
        Vector2 mousePos = mousePosition();

        Grid mouseHoverGrid = gridSelector();

        Vector2Int gridCoord;
        bool nullGridValue;
        bool validItem = false;

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
            try
            {
                // Check if dropped at valid location or null grid
                if(nullGridValue || !(checkValidLocation(ItemHold, mouseHoverGrid.getXY(mousePos) + Gridoffset, mouseHoverGrid)))
                { throw new Exception(); }

                // Update position
                updateMoveInventory(ItemHold, mouseHoverGrid.getXY(mousePos) + Gridoffset, mouseHoverGrid);

                // If the item is placed in a different grid, remove it from the list.
                if(previousItemGrid != mouseHoverGrid) { changeLocationItem(ItemHold); }
            }
            catch(Exception e) // Code only runs if item is dropped in non grid area.
            {
                updateMoveInventory(ItemHold, oldLocation, previousItemGrid);
            }
            finally
            {
                // Always resets the flags

                // Reset Item hold
                ItemHold = defaultItem;
                itemHoldOffset = Vector3.zero;

                // Flags
                onFirstHold = true;
                holdItem = false;
                snapToCell = true;
            }


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
        // Check what grid it is using 

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

        resetItemFlag();
        destroyExternalInventoryImages();

        if (!selfInventory)
        {
            if (checkEmptyGrid()){ overworldItem.destroyItem(); }

            externalGrid.closeInventory();
        }
    }
}
