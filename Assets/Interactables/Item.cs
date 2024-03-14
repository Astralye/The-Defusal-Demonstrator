using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    protected override void Interact()
    {

        // Todo
        // Add to inventory

        PlayerData.getItem = true;

        Destroy(gameObject);
        // --> Do not destroy until in inventory

    }
}
