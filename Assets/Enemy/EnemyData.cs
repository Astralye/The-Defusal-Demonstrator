using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyData : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AIAgent agent;
    private float health;

    private void Start()
    {
        agent = GetComponent<AIAgent>();
        health = agent.aiConfig.health;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidBody in rigidBodies)
        {
            Hitbox hitbox = rigidBody.gameObject.AddComponent<Hitbox>();
            hitbox.setData(this);
        }
    }

    // Parameters -> Damage source.
    public void takeDamage(float amount,Vector3 direction)
    {
        health -= amount;

        if (health <= 0)
        {
            Die(direction);
        }
        else
        {
            animator.SetBool("onHit", true);
        }
    }

    void Die(Vector3 direction)
    {
        AiAgentDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiAgentDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateId.Death);

        GetComponentInParent<NavMeshAgent>().enabled = false;
        GetComponentInParent<AILocomotion>().enabled = false;
    }
}
