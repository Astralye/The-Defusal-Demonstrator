using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndLevel : Interactable
{
    protected override void Interact()
    {
        if (PlayerData.disarmed)
        {
            Debug.Log("EndLevel");
        }
    }
}
