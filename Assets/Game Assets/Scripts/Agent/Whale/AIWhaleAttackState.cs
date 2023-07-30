using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMovementAI;

public class AIWhaleAttackState : AIState
{
       
    private GameObject _teleportMarker = null;
    private GameObject _target;
    private AIAgentConfig _krillConfig;
    private float _chargeTimer = 0f;
    private float _teleportTimer = 0f;
    private float _spawnTimer = 0f;
    private float _updateTimer = 0f;
    private float _teleportCooldown = 15f;
    private float _spawnCooldown = 20f;
    protected bool _attacking = false;
    protected int _attack;
    private Vector3[] _nodes = new Vector3[4];

   

    public void Enter(AIAgent agent)
    {      
        _target = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Attack");
    }

    public void Exit(AIAgent agent)
    {

    }

    public AIStateID GetID()
    {
        return AIStateID.WhaleAttack;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        if (!_attacking)
        {
            _attack = Random.Range(0, 3);
            Debug.Log(_attack);
        }

        if (_attack == 2 && _spawnTimer >= _spawnCooldown)
        {
            if (_krillConfig.krill.Count < 10)
            {
                SpawnEnemies(agent);
            }           
            _spawnTimer = 0f;
        }
        else if (_attack == 1 && _teleportTimer >= _teleportCooldown)
        {
            Teleport(agent);
        }
        else
        {           
            Charge(agent);           
        }

        if (_teleportTimer < _teleportCooldown)
        {
            _teleportTimer += Time.deltaTime;
        }

        if (_spawnTimer < _spawnCooldown)
        {
            _spawnTimer += Time.deltaTime;
        }
       
        

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
            agent.path.nodes[3] = _target.transform.position;
            agent.path.CalcDistances();
            _updateTimer = 0f;
        }
        else if(_updateTimer > 0.33f && _chargeTimer < 2)
        {
            agent.path.nodes[0] = agent.transform.position;
            agent.path.nodes[2] = agent.path.nodes[2];
            agent.path.nodes[3] = _target.transform.position;
            agent.path.CalcDistances();
            _updateTimer = 0f;
        }
        else if(_updateTimer > 0.33f)
        {
            agent.path.nodes[0] = agent.transform.position;
            agent.path.nodes[1] = agent.path.nodes[2];
            agent.path.nodes[2] = agent.path.nodes[3];
            agent.path.nodes[3] = _target.transform.position;
            agent.path.CalcDistances();
            _updateTimer = 0f;
        }

        if (_chargeTimer > 5)
        {
            _attacking = false;
            _chargeTimer = 0;
            agent.steeringBasics.maxVelocity = agent.steeringBasics.maxVelocity / 2;
        }

        

        Vector3 accel = agent.wallAvoidance.GetSteering();


        if (accel.magnitude < 0.005f)
        {
            accel = agent.followPath.GetSteering(agent.path);
        }

        agent.steeringBasics.Steer(accel);
        agent.steeringBasics.LookWhereYoureGoing();

        agent.path.Draw();

    }

    private void Teleport(AIAgent agent)
    {      
        if (_attacking == false)
        {
            agent.steeringBasics.maxVelocity = agent.steeringBasics.maxVelocity * 2;
            _nodes[0] = agent.transform.position;
            Vector3 direction = (agent.gameObject.transform.position - _target.transform.position).normalized;           
            direction = agent.gameObject.transform.position + direction * 3f + new Vector3(0f, 20f, 0f);

            for (int i = 1; i < _nodes.Length; i++)
            {
                _nodes[i] = direction;
            }
            agent.path = new LinePath(_nodes);
            agent.path.CalcDistances();
        }

        _attacking = true;
        _teleportTimer += Time.deltaTime;

        if (_teleportTimer > _teleportCooldown + 3f && _teleportTimer < _teleportCooldown + 5f)
        {
            if (_teleportMarker == null)
            {
                agent.GetComponent<MeshRenderer>().enabled = false;
                _teleportMarker = GameObject.Instantiate(agent.config.teleportEffect, agent.transform.position, agent.transform.rotation);
            }
            
        }
        else if (_teleportTimer > _teleportCooldown + 5f && _teleportTimer < _teleportCooldown + 7f)
        {
            if (_teleportMarker != null)
            {
                agent.transform.position = new Vector3((_target.transform.position - _teleportMarker.transform.position).normalized.x * 3f, _target.transform.position.y, (_target.transform.position - _teleportMarker.transform.position).normalized.z * 3f);
                agent.GetComponent<MeshRenderer>().enabled = true;
                _nodes[0] = agent.transform.position;
                for (int i = 1; i < _nodes.Length; i++)
                {
                    _nodes[i] = _target.transform.position;
                }
                agent.path = new LinePath(_nodes);
                agent.path.CalcDistances();
                GameObject.Destroy(_teleportMarker);
            }
        }
        else if (_teleportTimer > _teleportCooldown + 7f)
        {
            GameObject.Destroy(_teleportMarker);
            _attacking = false;
            _teleportTimer = 0;
            agent.steeringBasics.maxVelocity = agent.steeringBasics.maxVelocity / 2;
        }



       Vector3 accel = agent.wallAvoidance.GetSteering();


        if (accel.magnitude < 0.005f)
        {
            accel = agent.followPath.GetSteering(agent.path);
        }

        agent.steeringBasics.Steer(accel);
        agent.steeringBasics.LookWhereYoureGoing();

        agent.path.Draw();
    }

    private void SpawnEnemies(AIAgent agent)
    {
        for (int i = 0; i < Random.Range(4,6); i++)
        {
            Vector3 offset = new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), Random.Range(-5, 6));
            GameObject.Instantiate(agent.config.enemyPrefab, agent.gameObject.transform.position + offset, Quaternion.identity);
        }
        return;
    }


}
