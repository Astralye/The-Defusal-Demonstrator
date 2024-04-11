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

    private bool interacted;
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
        initItem();
        inventoryPosition = new Vector2Int(1, 0);

        itemID = Random.Range(0, 5000);
        interacted = false;
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
            // This would expand with other items.
        }
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

        // Todo
        // Add to inventory

        PlayerData.getItem = true;

        if (interacted) { return; }
        inventory.overworldInventory(this);
        interacted = true;

        Destroy(gameObject);
        // --> Do not destroy until in inventory

    }

    public ItemList.Items getItemType()
    {
        return itemType;
    }
}
