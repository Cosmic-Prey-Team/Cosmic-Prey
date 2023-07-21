using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMovementAI;

public class AIKrillFollowState : AIState
{
    private SteeringBasics steeringBasics;
    private FollowPath followPath;
    private Cohesion cohesion;
    private Separation separation;
    private GameObject whale;
    private AIAgent whaleAgent;
    private List<MovementAIRigidbody> krillInstances;
    private int castFails = 0;
    private float scanTimer = 0f;
    private AISensor sensor;
    [SerializeField]
    float rotationLimit = 30f;
    [SerializeField]
    float distanceLimit = 6f;
  


    public void Enter(AIAgent agent)
    {
        steeringBasics = agent.GetComponent<SteeringBasics>();
        followPath = agent.GetComponent<FollowPath>();
        cohesion = agent.GetComponent<Cohesion>();
        separation = agent.GetComponent<Separation>();
        whale = GameObject.FindGameObjectWithTag("Whale");
        whaleAgent = whale.GetComponent<AIAgent>();

        krillInstances = new List<MovementAIRigidbody>();
        //this only needs to run once, dont like this
        if (!agent.config.krill.Contains(whale.GetComponent<MovementAIRigidbody>()))
        {
            agent.config.krill.Add(whale.GetComponent<MovementAIRigidbody>());
        }
        agent.config.krill.Add(agent.gameObject.GetComponent<MovementAIRigidbody>());
        foreach (MovementAIRigidbody r in agent.config.krill)
        {
            krillInstances.Add(r);
        }
        krillInstances.Remove(agent.gameObject.GetComponent<MovementAIRigidbody>());
        sensor = agent.GetComponent<AISensor>();
    }

    public void Exit(AIAgent agent)
    {

    }

    public AIStateID GetID()
    {
        return AIStateID.KrillFollow;
    }

    public void Update(AIAgent agent)
    {
        
        if (!agent.enabled)
        {
            return;
        }
   

        if (whaleAgent.stateMachine.currentState.ToString() == "WhaleAttack")
        {
            agent.stateMachine.ChangeState(AIStateID.KrillAttack);
        }
        if (krillInstances.Count+1 < agent.config.krill.Count)
        {
            krillInstances.Add(agent.config.krill[agent.config.krill.Count - 1]);
        }

        

        scanTimer -= Time.deltaTime;
        
        if (agent.path.nodes.Length == 0 || agent.followPath.IsAtEndOfPath(agent.path))
        {

            Vector3[] nodes = new Vector3[1];

            Vector3 direction = (whale.transform.position - agent.gameObject.transform.position).normalized;
            float angleY = Random.Range(-rotationLimit, rotationLimit);
            float angleZ = Random.Range(-rotationLimit, rotationLimit);
            float distance = Random.Range(2, distanceLimit);
            direction = Quaternion.Euler(0, angleY, angleZ / 1.5f) * direction * distance;
            Debug.Log(direction);
            Debug.DrawRay(agent.gameObject.transform.position, direction, Color.yellow, 5.0f);
            if (!Physics.Raycast(agent.gameObject.transform.position, direction, distanceLimit, agent.config.occlusionLayers))
            {
                nodes[0] = (direction + agent.gameObject.transform.position) * .95f;
                //nodes[0] = whaleAgent.config.destination.transform.position;
                agent.path = new LinePath(nodes);
                agent.path.CalcDistances();

                castFails = 0;
                rotationLimit = 30f;
            }
            else if (castFails++ > 7)
            {
                Debug.Log("huh");
                nodes[0] = whaleAgent.config.destination.transform.position;
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
           
            Vector3 accel = Vector3.zero;
            accel += agent.wallAvoidance.GetSteering();
            if (accel.magnitude < 0.005f)
            {
                
                Debug.Log(accel);
                accel = agent.steeringBasics.Arrive(whale.transform.position);
                Debug.Log(accel);
                //accel += cohesion.GetSteering(krillInstances) * 1.5f;
                Debug.Log(accel);
                accel += separation.GetSteering(krillInstances);
                Debug.Log(accel);
                
                
                Debug.Log("done");
                //accel = agent.followPath.GetSteering(agent.path);
                //accel = agent.steeringBasics.Arrive(whaleAgent.config.destination.transform.position);
            }
            
            
 
            agent.steeringBasics.Steer(accel);
            agent.steeringBasics.LookWhereYoureGoing();
            
            agent.path.Draw();
        }

        if (scanTimer < 0)
        {

            scanTimer = 0.5f;
            sensor.Scan();
            GameObject[] player = sensor.Filter(new GameObject[1], "Player");

            if (player[0] != null)
            {
                agent.stateMachine.ChangeState(AIStateID.KrillAttack);
            }
        }

    }


}
