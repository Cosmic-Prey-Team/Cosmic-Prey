using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMovementAI;

public class AIKrillAttackState : AIState
{
       
    private GameObject _target;
    private FollowPath followPath;
    private Cohesion cohesion;
    private Separation separation;
    private List<MovementAIRigidbody> krillInstances;
    private float _chargeTimer = 0f;  
    private float _updateTimer = 0f;
    protected bool _attacking = false;
    protected int _attack;
    private Vector3[] _nodes = new Vector3[3];

   

    public void Enter(AIAgent agent)
    {
        krillInstances = new List<MovementAIRigidbody>();
        _target = GameObject.FindGameObjectWithTag("Player");
        followPath = agent.GetComponent<FollowPath>();
        cohesion = agent.GetComponent<Cohesion>();
        separation = agent.GetComponent<Separation>();

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
            agent.steeringBasics.maxVelocity = agent.steeringBasics.maxVelocity * 2;
            _nodes[0] = agent.transform.position;
            for (int i = 1; i < _nodes.Length; i++)
            {
                _nodes[i] = _target.transform.position;
            }
            agent.path = new LinePath(_nodes);
            agent.path.CalcDistances();
        }

        _attacking = true;

        _chargeTimer += Time.deltaTime;
        _updateTimer += Time.deltaTime;

        if (_updateTimer > 0.33f && _chargeTimer < 1)
        {
            agent.path.nodes[0] = agent.transform.position;
            agent.path.nodes[2] = _target.transform.position;
            agent.path.CalcDistances();
            _updateTimer = 0f;
        }
        else if(_updateTimer > 0.33f)
        {
            agent.path.nodes[0] = agent.transform.position;
            agent.path.nodes[1] = agent.path.nodes[2];
            agent.path.nodes[2] = _target.transform.position;
            agent.path.CalcDistances();
            _updateTimer = 0f;
        }

        if (_chargeTimer > 5)
        {
            _attacking = false;
            _chargeTimer = 0;
            agent.steeringBasics.maxVelocity = agent.steeringBasics.maxVelocity / 2;
        }


        Vector3 accel = Vector3.zero;
        accel += followPath.GetSteering(agent.path) * 1.5f;
        accel += agent.wallAvoidance.GetSteering();
        //accel += cohesion.GetSteering(krillInstances) * 1.5f;
        accel += separation.GetSteering(krillInstances);

        agent.steeringBasics.Steer(accel);
        agent.steeringBasics.LookWhereYoureGoing();

        agent.path.Draw();
        

    }

   

    


}
