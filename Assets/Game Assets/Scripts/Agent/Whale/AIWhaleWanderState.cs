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
    private int castFails = 0;
    private float scanTimer = 0f;
    private AISensor sensor;
    [SerializeField]
    float rotationLimit = 30f;
    [SerializeField]
    float distanceLimit = 6f;


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

        if (agent.path.nodes.Length == 0 || agent.followPath.IsAtEndOfPath(agent.path))
        {
            if (destination == null)
            {
                destination = destinations[Random.Range(0, destinations.Length - 1)].transform;
                agent.config.destination = destination;
            }

            Vector3[] nodes = new Vector3[1];

            Vector3 direction = (destination.transform.position - agent.gameObject.transform.position).normalized;
            float angleY = Random.Range(-rotationLimit, rotationLimit);
            float angleZ = Random.Range(-rotationLimit, rotationLimit);
            float distance = Random.Range(2, distanceLimit);
            direction = Quaternion.Euler(0, angleY, angleZ / 1.5f) * direction * distance;

            Debug.DrawRay(agent.gameObject.transform.position, direction, Color.yellow, 5.0f);
            if (!Physics.Raycast(agent.gameObject.transform.position, direction, distanceLimit, agent.config.occlusionLayers))
            {
                if ((destination.transform.position - agent.gameObject.transform.position).magnitude < distanceLimit / 2)
                {
                    nodes[0] = destination.transform.position;
                }
                else
                {
                    nodes[0] = (direction + agent.gameObject.transform.position) * .95f;
                }

                agent.path = new LinePath(nodes);
                agent.path.CalcDistances();

                castFails = 0;
                rotationLimit = 30f;
            }
            else if (castFails++ > 7)
            {                
                nodes[0] = destination.transform.position;
                agent.path = new LinePath(nodes);
                agent.path.CalcDistances();
            }
        }
        else if (scanTimer < 0 && castFails > 7)
        {
            if (agent.path.Length > 0)
            {
                agent.path.RemoveNodes();
            }       
        }
        else
        {
            Vector3 accel = agent.wallAvoidance.GetSteering();

            if (accel.magnitude < 0.005f)
            {
                accel = agent.followPath.GetSteering(agent.path);
            }

            agent.steeringBasics.Steer(accel);
            agent.steeringBasics.LookWhereYoureGoing();
            
            agent.path.Draw();
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
