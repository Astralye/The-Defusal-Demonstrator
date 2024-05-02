using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public float health;

    private Animator animator;

    private void Start()
    {
        health = 100;
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        checkHealth();

        //takeDamage();
    }

    // This could be put in its own class
    // This would be only the functions to enemy data

    private void checkHealth()
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
        }
    }

    // Parameters -> Damage source.
    private void takeDamage()
    {

        float damage = 0.1f;
        health -= damage;
    }

    private void checkIfHit()
    {

    }

}
