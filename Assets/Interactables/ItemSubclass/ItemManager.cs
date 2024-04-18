using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using static ItemList;
using System;

// This is used for loading and storing item data

// inbuilt JsonUtility doesnt allow custom Json datastructures for storing all the
// item data.
// Github Repo to LitJson:
// https://github.com/Mervill/UnityLitJson

// If keyexception, make sure validate json:
// https://jsonlint.com/

public class ItemManager : MonoBehaviour
{

    private string jsonString;
    private JsonData jsondata;

    private void Awake()
    {
        jsonString = File.ReadAllText(Application.dataPath + "/Interactables/ItemSubclass/ItemData.json");
        jsondata = JsonMapper.ToObject(jsonString);
    }

    private JsonData getItemData(Item item, string dataType)
    {
        string itemCategory = item.getCategory().ToString();
        string itemName = item.getItemType().ToString();

        JsonData itemData = jsondata[itemCategory];

        for (int i = 0; i < jsondata[itemCategory].Count; i++){
            if (itemData[i]["itemBaseData"]["itemName"].ToString() == itemName){
                return itemData[i][dataType];
            }
        }

        return null;
    }


    // json object 'itemBaseData'
    public void setBaseData(Item item)
    {
        Awake();

        JsonData itemData = getItemData(item, "itemBaseData");
        if (itemData == null) { return; }

        item.setBaseValues(
            itemData["itemName"].GetString(),
            itemData["Item_Description"].GetString(),
            (int)itemData["item_ID"],
            new Vector2Int( (int)itemData["Dimensions"][0].GetNatural(), (int)itemData["Dimensions"][1].GetNatural())
            );
    }

    // json object 'itemWeaponData'
    public void setGeneralWeaponData(Weapons item)
    {
        Awake();

        JsonData itemData = getItemData(item, "itemWeaponData");

        if (itemData == null) { return; }
        
        // Damage
        // Attack range
        // Attack Speed

        item.setWeaponValues(
            (int)itemData["Damage"],
            (int)itemData["Attack_Range"],
            (float)itemData["Attack_Speed"]
            );

    }

    // json object 'itemRangedData'
    public void setRangedWeaponData(RangedWeapon item)
    {
        Awake();

        JsonData itemData = getItemData(item, "itemRangedData");

        if (itemData == null) { return; }

        // Ammo_Type
        // Attack_Range
        // Attack_Speed

        item.setRangedValues(
            itemData["Ammo_Type"].GetString(),
            (int)itemData["Ammo_Capacity"],
            (int)itemData["Weapon_Spread"]
            );
    }



}