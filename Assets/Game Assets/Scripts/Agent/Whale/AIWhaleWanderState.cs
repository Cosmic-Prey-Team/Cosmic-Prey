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
    private FlyingController flyingController;

    public void Enter(AIAgent agent)
    {
        destinations = GameObject.FindGameObjectsWithTag("PermWaypoint");
        sensor = agent.GetComponent<AISensor>();
        flyingController = agent.GetComponent<FlyingController>();
        flyingController.delay = 2f;
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
                Debug.Log("Saw Player");
                agent.stateMachine.ChangeState(AIStateID.WhaleFlee);
            }
            else if (food[0] != null)
            {
                agent.config.destination = destination = food[0].transform;              
            }
        }

    }


    
}
