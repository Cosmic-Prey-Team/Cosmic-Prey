using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class AIWhaleWanderState : AIState
{
    private GameObject[] destinations;
    private Transform destination;
    private GameObject moveTarget;
    private Rigidbody whaleRB;
    
    [SerializeField]
    float rotationLimit = 30f;
    [SerializeField]
    float distanceLimit = 5f;

    public void Enter(AIAgent agent)
    {
        whaleRB = agent.gameObject.GetComponent<Rigidbody>();
        //Probably a cleaner way to do this than use tags
        destinations = GameObject.FindGameObjectsWithTag("PermWaypoint");
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

        if (moveTarget == null)
        {
            if (destination == null)
            {
                destination = destinations[Random.Range(0, destinations.Length)].transform;
            }

            if ((destination.transform.position - agent.gameObject.transform.position).magnitude < distanceLimit)
            {
                moveTarget = GameObject.Instantiate(agent.config.Waypoint, destination.transform.position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                // randomize position of target
                Vector3 direction = (destination.transform.position - agent.gameObject.transform.position).normalized;
                Vector2 range = new Vector2(1, 0);
                float angleY = Random.Range(-rotationLimit, rotationLimit);
                float angleZ = Random.Range(-rotationLimit, rotationLimit);
                float distance = range.x * Random.Range(1, distanceLimit);
                direction = Quaternion.Euler(0, angleY, angleZ/1.5f) * direction * distance;

                moveTarget = GameObject.Instantiate(agent.config.Waypoint, direction + agent.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            }
        }

        
        Vector3 move = agent.gameObject.transform.position + (moveTarget.transform.position - agent.gameObject.transform.position).normalized * (agent.config.Speed * Time.deltaTime);
        
        whaleRB.MovePosition(move);
    }
}
