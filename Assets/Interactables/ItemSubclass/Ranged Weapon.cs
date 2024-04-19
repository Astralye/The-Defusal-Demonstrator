using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapons
{
    private AmmoType ammo_Type;
    private float weapon_Spread;
    private int total_ammo;
    private int current_ammo;

    private void Awake()
    {
        itemManager.setRangedWeaponData(this);

        //Debug.Log("Ranged:" + ammo_Type + "," + weapon_Spread + "," + total_ammo);

        base.Awake();
    }

    public void setRangedValues(string ammoType, int ammoCapacity, int weaponSpread)
    {
        ammo_Type = getAmmoType(ammoType);
        weapon_Spread = weaponSpread;
        total_ammo = ammoCapacity;
        current_ammo = 0;
    }

    private AmmoType getAmmoType(string type)
    {
        return (AmmoType)Enum.Parse(typeof(AmmoType), type);
    }

}

public enum AmmoType
{
    Pistol,
    Shotgun,
    AR
}
