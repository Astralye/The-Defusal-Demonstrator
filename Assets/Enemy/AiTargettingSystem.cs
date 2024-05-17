using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTargettingSystem : MonoBehaviour
{
    public float memorySpan = 3.0f;
    public bool chasePlayer = false;

    AiSensoryMemory memory = new AiSensoryMemory(10);
    AiSensor sensor;

    // Start is called before the first frame update
    void Start()
    {
        sensor = GetComponent<AiSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        memory.UpdateSenses(sensor);
        memory.ForgetMemories(memorySpan);

        chasePlayer = memory.StillRememberPlayer();
    }

    private void OnDrawGizmos()
    {
        foreach(var memory in memory.memories)
        {
            Color color = Color.red;

            Gizmos.color = color;
            Gizmos.DrawSphere(memory.position, 1.0f);
        }
    }

}
