using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    
    private List<Item> playerInventory = new List<Item>();


    public static bool disarmed;
    public static bool enablePlayerMovement;
    public static bool getItem;
    // Start is called before the first frame update
    void Start()
    {
        disarmed = false;
        enablePlayerMovement = true;
        getItem = false;
    }

    public List<Item> getPlayerInventory()
    {
        return playerInventory;
    }
}
