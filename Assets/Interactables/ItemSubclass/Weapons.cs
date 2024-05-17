using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Item
{
    protected float damage_Per_Hit;
    protected float attack_range;
    protected float attack_speed;

    protected void Awake()
    {
        itemManager.setGeneralWeaponData(this);
        
        //Debug.Log("Weapon:" + damage_Per_Hit + "," + attack_range + "," + attack_speed);

        base.Awake();
    }

    public void setWeaponValues(int damage, int attackRange, float attackSpeed)
    {
        damage_Per_Hit = damage;
        attack_range = attackRange;
        attack_speed = attackSpeed;
    }

    public float getDamage()
    {
        return damage_Per_Hit;
    }

    public float getAttackRange()
    {
        return attack_range;
    }

    public float getAttackSpeed()
    {
        return attack_speed;
    }

}
