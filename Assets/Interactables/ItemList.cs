
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
        PistolAmmo,
        ShotgunAmmo,


        // Melee Weapons

        Bat,
        Wrench,
    }

    public enum ItemCategory{ // used for identification which constructor to use
        Ranged_Weapon,
        Melee_Weapon,

        Ammo,
    }

    // Need to load JSON file,
    // This would contain the data for the construction of the objects.
}
