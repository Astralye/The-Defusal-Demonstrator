using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
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
}
