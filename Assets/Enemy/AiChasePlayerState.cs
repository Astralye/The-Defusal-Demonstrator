using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.Android;

public class AiChasePlayerState : AIState
{
    float timer = 0.0f;
    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }
    public void Enter(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
    }


    public void Update(AIAgent agent)
    {
        if (!agent.enabled) { return; }

        timer -= Time.deltaTime;

        float sqrdist = (agent.playerTransform.position - agent.navMeshAgent.destination).sqrMagnitude;

        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if (playerDirection.magnitude > agent.aiConfig.maxPlayerDistRad)
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
            timer = agent.aiConfig.maxTime;
            return;
        }

        // if AI still sees player or remembers them 
        if (timer < 0.0f)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;

            if (!agent.targeting.chasePlayer)
            {
                agent.stateMachine.ChangeState(AiStateId.Idle);
                return;
            }

            timer = agent.aiConfig.maxTime;
        }
    }
    private bool FindPlayer(AIAgent agent)
    {
        if (agent.sensor.Objects.Count > 0)
        {
            return true;
        }
        return false;
    }
}
