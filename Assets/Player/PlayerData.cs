using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    private List<Item> playerInventory;


    public static bool disarmed;
    public static bool enablePlayerMovement;
    public static bool getItem;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new List<Item>();

        disarmed = false;
        enablePlayerMovement = true;
        getItem = false;
    }

    public List<Item> getPlayerInventory()
    {
        return playerInventory;
    }

    public void setInventory(List<Item> data)
    {
        playerInventory = data;
    }
}
