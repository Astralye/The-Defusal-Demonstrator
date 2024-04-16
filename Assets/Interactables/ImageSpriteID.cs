using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSpriteID : MonoBehaviour
{

    private int itemID;

    public void setID(int id)
    {
        itemID = id;
    }

    public int getID()
    {
        return itemID;
    }
}
