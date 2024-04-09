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

    private Orientation orientation;
    public Vector2Int Dimensions;
    public Vector2Int inventoryPosition;
    
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private ItemList.Items itemType;

    [SerializeField]
    private Color color;

    private void Start()
    {
        initItem();

        inventoryPosition = new Vector2Int(0, 0);
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

        inventory.overworldInventory(this);

        Destroy(gameObject);
        // --> Do not destroy until in inventory

    }

    public ItemList.Items getItemType()
    {
        return itemType;
    }
}
