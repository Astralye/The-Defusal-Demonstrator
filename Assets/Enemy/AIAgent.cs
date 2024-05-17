using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AiAgentConfig aiConfig;
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public AiStateId initialState;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public AiSensor sensor;
    [HideInInspector] public AiTargettingSystem targeting;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ragdoll = GetComponent<Ragdoll>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        sensor = GetComponent<AiSensor>();
        targeting = GetComponent<AiTargettingSystem>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiAgentDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
