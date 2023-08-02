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
    private GameObject whale;
    private AIAgent whaleAgent;
    private List<MovementAIRigidbody> krillInstances;
    private float scanTimer = 0f;
    private AISensor sensor;
    [SerializeField]
    float rotationLimit = 30f;
    [SerializeField]
    float distanceLimit = 6f;
    private FlyingController _flyingController;
    private AStarAgent _aStarAgent;
    private WorldManager worldManager;


    public void Enter(AIAgent agent)
    {
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
        worldManager = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        _flyingController = agent.GetComponent<FlyingController>();
        _aStarAgent = agent.GetComponent<AStarAgent>();
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

        if (agent.config.destination == null)
        {                
            Vector3 offset = new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), Random.Range(-5, 6));
            Vector3 destination = worldManager.GetClosestPointWorldSpace(whale.transform.position + offset).WorldPosition;           
            agent.config.destination = GameObject.Instantiate(agent.config.Waypoint, destination, Quaternion.identity).transform;
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
