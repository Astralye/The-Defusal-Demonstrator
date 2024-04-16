using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{

    // Give it an image or colour for the inventory

    private enum Orientation
    {
        Vertical,
        Horizontal
    }

    // When interacting the code runs a couple times from interaction.
    private bool runOnce;

    private bool firstInsance;
    private Orientation orientation;
    public Vector2Int Dimensions;
    public Vector2Int inventoryPosition;

    public int itemID;
    
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private ItemList.Items itemType;

    [SerializeField]
    private Sprite spriteTexture;

    public Item()
    {
        orientation = Orientation.Vertical;
        Dimensions = Vector2Int.zero;
        inventoryPosition = Vector2Int.zero;
        itemType = ItemList.Items.None;
        itemID = 0;
    }

    private void Start()
    {
        runOnce = true;
        initItem();

        itemID = Random.Range(0, 5000);
        firstInsance = true;
    }

    // Need to determine what items properties it has.
    private void initItem()
    {
        switch (itemType)
        {
            case ItemList.Items.Pistol:
                orientation = Orientation.Horizontal;
                Dimensions = new Vector2Int(3, 2);
                break;

            case ItemList.Items.PistolAmmo:
                orientation = Orientation.Horizontal;
                Dimensions = new Vector2Int(1, 1);
                break;
            // This would expand with other items.
        }

        // This can be randomized
        inventoryPosition = new Vector2Int(0, 0);
    }

    public Sprite getSpriteTexture()
    { 
        return spriteTexture;
    }

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

    public void destroyItem()
    {
        PlayerData.getItem = true;

        MoveItem();
    }

    public void resetRunOnce()
    {
        runOnce = true;
    }

    public ItemList.Items getItemType()
    {
        return itemType;
    }

    // I cannot delete inventory items otherwise it will cause errors.
    // I will just put them elsewhere in the level.
    public void MoveItem()
    {
        gameObject.transform.position = GameObject.Find("InventoryLocation").transform.position;
    }
}
