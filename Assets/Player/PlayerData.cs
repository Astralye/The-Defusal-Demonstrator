using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    private List<Item> playerInventory;

    private Weapons itemOut;

    public static bool disarmed;
    public static bool enablePlayerMovement;
    public static bool getItem;


    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new List<Item>();

        disarmed = false;
        enablePlayerMovement = true;
        getItem = false;

        animator = GameObject.Find("PlayerModel").GetComponent<Animator>();
    }

    public List<Item> getPlayerInventory()
    {
        return playerInventory;
    }

    public void setInventory(List<Item> data)
    {
        playerInventory = data;
    }

    private void Update()
    {
        checkAnimationFlags();

    }
    private void checkAnimationFlags()
    {

        foreach (Item item in playerInventory)
        {

            switch (item.name)
            {
                case "Pistol":
                    {
                        animator.SetBool("holdPistol", true);
                        break;
                    }
            }
            
        }
    }
}
