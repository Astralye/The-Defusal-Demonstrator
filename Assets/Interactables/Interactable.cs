using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The objects that the player looks at when pressed the interact key
public abstract class Interactable : MonoBehaviour
{
    public bool isEnabled = true;

    [SerializeField]
    public string hoverMessage;

    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // Template function
    }
}
