using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMovementAI;

public class AIWhaleFleeState : AIState
{
    private GameObject _target;
    private int castFails = 0;
    private float scanTimer = 0f;

    [SerializeField]
    float rotationLimit = 40f;
    [SerializeField]
    float distanceLimit = 16f;

   

    public void Enter(AIAgent agent)
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        agent.steeringBasics.maxVelocity = agent.steeringBasics.maxVelocity * 1.25f;
    }

    public void Exit(AIAgent agent)
    {
        agent.steeringBasics.maxVelocity = agent.steeringBasics.maxVelocity / 1.25f;
    }

    public AIStateID GetID()
    {
        return AIStateID.WhaleFlee;
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
            
            Vector3[] nodes = new Vector3[1];

            Vector3 direction = (agent.gameObject.transform.position - _target.transform.position).normalized;
            float angleY = Random.Range(-rotationLimit, rotationLimit);
            float angleZ = Random.Range(-rotationLimit, rotationLimit);
            float distance = Random.Range(8, distanceLimit);
            direction = Quaternion.Euler(0, angleY / 0.5f, angleZ / 1.5f) * direction * distance;

            RaycastHit hit;
            Debug.DrawRay(agent.gameObject.transform.position, direction, Color.yellow, 5.0f);
            if (!Physics.Raycast(agent.gameObject.transform.position, direction, out hit, distanceLimit, agent.config.occlusionLayers))
            {
                
                nodes[0] = (direction + agent.gameObject.transform.position) * .95f;
                

                agent.path = new LinePath(nodes);
                agent.path.CalcDistances();

                castFails = 0;
                rotationLimit = 30f;
            }
            else if (castFails++ > 7)
            {
                scanTimer = 0.5f;
                nodes[0] = hit.collider.transform.position + (agent.gameObject.transform.position - hit.collider.transform.position).normalized + hit.collider.bounds.size * .75f;
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

    }

    
}
