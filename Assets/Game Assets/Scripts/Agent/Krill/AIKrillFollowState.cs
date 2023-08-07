using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class AIKrillFollowState : AIState
{
    private GameObject whale;
    private AIAgent whaleAgent;
    private float scanTimer = 0f;
    private AISensor sensor;
    [SerializeField]
    float rotationLimit = 30f;
    [SerializeField]
    float distanceLimit = 6f;
    private KrillFlyingController _flyingController;
    private AStarAgent _aStarAgent;
    private WorldManager worldManager;


    public void Enter(AIAgent agent)
    {
        whale = GameObject.FindGameObjectWithTag("Whale");
        whaleAgent = whale.GetComponent<AIAgent>();

        
        sensor = agent.GetComponent<AISensor>();
        worldManager = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        _flyingController = agent.GetComponent<KrillFlyingController>();
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
