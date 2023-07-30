using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMovementAI;

public class AIWhaleWanderState : AIState
{
    private GameObject[] destinations;
    private Transform destination;
    private float scanTimer = 0f;
    private AISensor sensor;


    public void Enter(AIAgent agent)
    {
        destinations = GameObject.FindGameObjectsWithTag("PermWaypoint");
        sensor = agent.GetComponent<AISensor>();
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AIStateID GetID()
    {
        return AIStateID.WhaleWander;
    }

    public void Update(AIAgent agent)
    {
        
        if (!agent.enabled)
        {
            return;
        }

        scanTimer -= Time.deltaTime;
       
        if (destination == null)
        {
            destination = destinations[Random.Range(0, destinations.Length - 1)].transform;
            agent.config.destination = destination;
        }            

        if (scanTimer < 0)
        {
            
            scanTimer = 0.5f;
            sensor.Scan();
            GameObject[] food = sensor.Filter(new GameObject[1], "Food");
            GameObject[] player = sensor.Filter(new GameObject[1], "Player");
            if (player[0] != null)
            {
                agent.stateMachine.ChangeState(AIStateID.WhaleAttack);
            }
            else if (food[0] != null)
            {
                Vector3[] nodes = new Vector3[1];
                nodes[0] = food[0].transform.position;
                agent.path = new LinePath(nodes);
                agent.path.CalcDistances();
               
            }
        }

    }

    
}
