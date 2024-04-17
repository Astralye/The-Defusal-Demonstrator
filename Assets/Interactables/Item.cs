using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemList;

public class Item : Interactable
{
    // Base Item properties
    // ---------------------------------------------------------------------
    private Vector2Int Dimensions;
    private string itemName;

    public int itemID;

    [SerializeField]
    private Sprite spriteTexture;

    [SerializeField]
    protected Items itemType;
    
    [SerializeField]
    protected ItemCategory itemCategory;

    // Inventory variables
    // -----------------------------------------------------------------------

    private bool runOnce;
    private enum Orientation
    {
        Vertical,
        Horizontal
    }


    private Orientation orientation;
    public Vector2Int inventoryPosition;

    
    [SerializeField]
    private Inventory inventory;

    // -------------------------------------------------------------------------


    [SerializeField]
    protected ItemManager itemManager;

    public Item()
    {
        orientation = Orientation.Vertical;
        Dimensions = Vector2Int.zero;
        inventoryPosition = Vector2Int.zero;
        itemType = ItemList.Items.None;
        itemCategory = ItemCategory.None;
        itemID = 0;
    }

    protected void Awake()
    {
        runOnce = true;

        itemManager.setBaseData(this);

        // This can be randomized
        inventoryPosition = new Vector2Int(0, 0);


        Debug.Log("Item:" + itemName + "," + itemID + "," + itemType);

    }

    public Sprite getSpriteTexture()
    { 
        return spriteTexture;
    }

    // Inventory Functions
    // -------------------------------------------------------------------------------------------

    private void rotate()
    {
        Dimensions = new Vector2Int(Dimensions.y, Dimensions.x);
    }

    // Item within the inventory
    protected override void Interact()
    {
        if (!runOnce) { return; }
        runOnce = false;

        inventory.overworldInventory(this);

    }

    // Setter
    // -------------------------------------------------------------------------------------------

    public void setBaseValues(string name, int id, Vector2Int dim)
    {
        itemName = name;
        itemID = id;
        Dimensions = dim;
    }


    // Getters 
    // -------------------------------------------------------------------------------------------
    public ItemList.Items getItemType(){ return itemType; }
    public ItemList.ItemCategory getCategory(){ return itemCategory; }

    public Vector2 getItemDimensions() { return Dimensions; }

    // Resetters
    // -------------------------------------------------------------------------------------------

    public void MoveItem()
    {

        gameObject.transform.position = GameObject.Find("PlayerInventory").transform.position;
    }
    public void destroyItem()
    {
        PlayerData.getItem = true;

        MoveItem();
    }

    public void resetRunOnce()
    {
        runOnce = true;
    }


}
