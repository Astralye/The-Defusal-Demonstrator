using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgentDeathState : AIState
{
    public Vector3 direction;

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }
    public void Enter(AIAgent agent)
    {
        agent.ragdoll.ActiveRagdoll();
        direction.y = 0;
        agent.ragdoll.ApplyForce(direction * agent.aiConfig.dieForce);
    }

    public void Exit(AIAgent agent)
    {
    }

    public void Update(AIAgent agent)
    {
    }
}
