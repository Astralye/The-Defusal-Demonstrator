using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndLevel : Interactable
{
    [SerializeField]
    public PlayerData playerData;

    protected override void Interact()
    {
        if (playerData.disarmed)
        {
            Debug.Log("EndLevel");
        }
    }
}
