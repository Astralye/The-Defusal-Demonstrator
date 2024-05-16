using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadCode : Interactable
{
    [SerializeField] public int keyValue = -1;

    protected override void Interact()
    {
        // Send data to Keypad
        GetComponentInParent<Keypad>().setKey(keyValue);
    }

}
