using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private EnemyData enemyData;

    public void setData(EnemyData enemy)
    {
        enemyData = enemy;
    }

    public void OnRaycastHit(float damage,Vector3 direction)
    {
        enemyData.takeDamage(damage,direction);
    }
}
