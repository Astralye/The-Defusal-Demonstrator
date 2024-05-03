using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyData : MonoBehaviour
{
    public float health;
    private bool disableEnemy;

    [SerializeField] private Animator animator;

    private void Start()
    {
        health = 100;
        disableEnemy = false;
    }

    private void Update()
    {
        if (disableEnemy) { return; }


        // Enemy code
    }

    // Parameters -> Damage source.
    public void takeDamage(float amouont)
    {
        health -= amouont;


        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetBool("onHit", true);
        }
    }

    void Die()
    {
        health = 0;
        animator.SetBool("isDead", true);

        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInParent<NavMeshAgent>().enabled = false;
        GetComponentInParent<AILocomotion>().enabled = false;

        disableEnemy = true;
    }
}
