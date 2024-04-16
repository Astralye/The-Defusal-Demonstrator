using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapons
{
    private AmmoType ammo_Type;
    private float weapon_Spread;
    private float total_ammo;
    private float current_ammo;

    public RangedWeapon(float damage = 0, float range = 0, float speed = 0)
    {
        damage_Per_Hit = damage;
        attack_range = range;
        attack_speed = speed;
    }
}

public enum AmmoType
{
    Pistol,
    Shotgun,
    AR
}
