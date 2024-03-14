using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The objects that the player looks at when pressed the interact key
public abstract class Interactable : MonoBehaviour
{
    protected bool isEnabled = true;
    protected bool toggleOn = true;

    public string hoverMessage;

    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // Template function
    }

    public bool getEnabled() { return isEnabled; }
    public bool getToggle() { return toggleOn; }
}
