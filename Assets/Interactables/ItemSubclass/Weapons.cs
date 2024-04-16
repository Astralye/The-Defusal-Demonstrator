using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Item
{
    protected float damage_Per_Hit;
    protected float attack_range;
    protected float attack_speed;

    public Weapons(float damage = 0, float range = 0, float speed = 0)
    {
        damage_Per_Hit = damage;
        attack_range = range;
        attack_speed = speed;
    }
}
