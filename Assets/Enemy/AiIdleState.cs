using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AIState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Enter(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
    }


    public void Update(AIAgent agent)
    {
        // If player is too far.
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if(playerDirection.magnitude > agent.aiConfig.maxPlayerDistRad)
        {
            return;
        }

        // Player in vision or is remembered
        if (FindPlayer(agent))
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }

    private bool FindPlayer(AIAgent agent)
    {
        if(agent.sensor.Objects.Count > 0)
        {
            return true;
        }
        return false;
    }
}
