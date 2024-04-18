
using UnityEngine;

public static class ItemList
{
    // This stores all the items that can be picked up in the inventory.
    // This makes it easier to name
    
    public enum Items{ // used for setting values
        None, // Used for empty spaces

        // Ranged Weapons
        Pistol,
        Shotgun,

        // Ammo
        Pistol_Ammo,
        Shotgun_Ammo,


        // Melee Weapons

        Bat,
        Crowbar,
    }

    public enum ItemCategory{ // used for identification which constructor to use
        None,

        Ranged_Weapon,
        Melee_Weapon,

        AmmoType,
    }

    // Need to load JSON file,
    // This would contain the data for the construction of the objects.
}
