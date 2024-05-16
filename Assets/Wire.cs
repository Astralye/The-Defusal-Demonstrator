using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : Interactable
{
    enum WireType
    {
        Red,
        Yellow,
        Black,
    };

    [SerializeField] WireType wireType;


    public void hoverItem()
    {

    }

    protected override void Interact()
    {
        Debug.Log(wireType);
    }
}
