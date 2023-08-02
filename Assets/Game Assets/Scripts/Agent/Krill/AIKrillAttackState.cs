using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMovementAI;

public class AIKrillAttackState : AIState
{
       
    private GameObject _target;
    private List<MovementAIRigidbody> krillInstances;
    private float _chargeTimer = 0f;  
    private float _updateTimer = 0f;
    protected bool _attacking = false;
    protected int _attack;
    private FlyingController _flyingController;
    private AStarAgent _aStarAgent;



    public void Enter(AIAgent agent)
    {
        krillInstances = new List<MovementAIRigidbody>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _flyingController = agent.GetComponent<FlyingController>();
        _aStarAgent = agent.GetComponent<AStarAgent>();

        foreach (MovementAIRigidbody r in agent.config.krill)
        {
            krillInstances.Add(r);
        }
        krillInstances.Remove(agent.gameObject.GetComponent<MovementAIRigidbody>());
    }

    public void Exit(AIAgent agent)
    {

    }

    public AIStateID GetID()
    {
        return AIStateID.KrillAttack;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        if (krillInstances.Count + 1 < agent.config.krill.Count)
        {
            krillInstances.Add(agent.config.krill[agent.config.krill.Count - 1]);
        }

            
        
        Charge(agent);           
                
    }

    private void Charge(AIAgent agent)
    {
        if (_attacking == false)
        {
            _aStarAgent.Speed = _aStarAgent.Speed * 2;
            agent.config.destination = _target.transform;
            _flyingController.delay = 0.33f;
        }

        _attacking = true;

        agent.config.destination = _target.transform;
        _chargeTimer += Time.deltaTime;

        if (_chargeTimer > 5)
        {
            Debug.Log("did it");
            _attacking = false;
            _chargeTimer = 0;
            _aStarAgent.Speed = _aStarAgent.Speed * 0.5f;
            _flyingController.delay = 2f;
        }

    }






}
