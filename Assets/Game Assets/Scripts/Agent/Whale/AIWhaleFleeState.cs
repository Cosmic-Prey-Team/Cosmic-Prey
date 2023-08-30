using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AIWhaleFleeState : AIState
{
    private GameObject _target;
    private AStarAgent _aStarAgent;
    private Transform _destination = null;
    [SerializeField]
    float rotationLimit = 40f;
    [SerializeField]
    float distanceLimit = 16f;
    private float _fleeTimer = 0f;


    public void Enter(AIAgent agent)
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _aStarAgent = agent.GetComponent<AStarAgent>();
        _aStarAgent.Speed = _aStarAgent.Speed * 1.25f;
    }

    public void Exit(AIAgent agent)
    {
        _aStarAgent.Speed = _aStarAgent.Speed / 1.25f;
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

        if (_destination == null)
        {
            Vector3 direction = (agent.gameObject.transform.position - _target.transform.position).normalized;
            float angleY = Random.Range(-rotationLimit, rotationLimit);
            float angleZ = Random.Range(-rotationLimit, rotationLimit);
            float distance = Random.Range(8, distanceLimit);
            direction = Quaternion.Euler(0, angleY / 0.5f, angleZ / 1.5f) * direction * distance;
            Point p = WorldManager.Instance.GetClosestPointWorldSpace(agent.gameObject.transform.position + direction);
            _destination = GameObject.Instantiate(agent.config.Waypoint, p.WorldPosition, agent.transform.rotation).transform;
            agent.config.destination = _destination;
        }

        _fleeTimer += Time.deltaTime;

        /**if ((agent.gameObject.transform.position - _target.transform.position).magnitude > 100f)
        {
            agent.stateMachine.ChangeState(AIStateID.WhaleWander);
        }
        else**/ if (_fleeTimer > 60f)
        {
            agent.stateMachine.ChangeState(AIStateID.WhaleAttack);
        }
    }

    
}
