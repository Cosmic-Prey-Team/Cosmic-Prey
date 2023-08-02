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
    private AIAgentConfig _krillConfig;
    private GameObject _teleportMarker = null;
    private Transform _destination = null;
    private GameObject _target;
    private float _chargeTimer = 0f;
    private float _teleportTimer = 0f;
    private float _spawnTimer = 0f;
    private float _updateTimer = 0f;
    private float _teleportCooldown = 15f;
    private float _spawnCooldown = 20f;
    protected bool _attacking = false;
    protected int _attack;
    private FlyingController _flyingController;
    private AStarAgent _aStarAgent;
    private Transform whaleModel;

    public void Enter(AIAgent agent)
    {      
        _target = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Attack");
        _flyingController = agent.GetComponent<FlyingController>();
        _aStarAgent = agent.GetComponent<AStarAgent>();
        _krillConfig = agent.config.enemyPrefab.GetComponent<AIAgent>().config;
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

    private void Teleport(AIAgent agent)
    {
        
        if (_attacking == false)
        {
            _aStarAgent.Speed = _aStarAgent.Speed * 2;
            _flyingController.delay = 0.33f;
            Vector3 direction = (agent.gameObject.transform.position - _target.transform.position).normalized;           
            direction = agent.gameObject.transform.position + direction * 5f + new Vector3(0f, 30f, 0f);
            _destination = GameObject.Instantiate(agent.config.Waypoint, direction, agent.transform.rotation).transform;
            agent.config.destination = _destination;
            _aStarAgent.Pathfinding(_destination.position);
            whaleModel = agent.GetComponentsInChildren<Transform>()[1];
        }

        _attacking = true;
        _teleportTimer += Time.deltaTime;

        
        if (_teleportTimer > _teleportCooldown + 3f && _teleportTimer < _teleportCooldown + 5f)
        {
            if (_teleportMarker == null)
            {
                whaleModel.gameObject.SetActive(false);
                _teleportMarker = GameObject.Instantiate(agent.config.teleportEffect, agent.transform.position, agent.transform.rotation);
            }
            
        }
        else if (_teleportTimer > _teleportCooldown + 5f && _teleportTimer < _teleportCooldown + 7f)
        {
            if (_teleportMarker != null)
            {
                agent.transform.position = new Vector3((_target.transform.position - _teleportMarker.transform.position).normalized.x * 3f, _target.transform.position.y, (_target.transform.position - _teleportMarker.transform.position).normalized.z * 3f);
                whaleModel.gameObject.SetActive(true);
                agent.config.destination = _target.transform;
                GameObject.Destroy(_teleportMarker);
            }
        }
        else if (_teleportTimer > _teleportCooldown + 7f)
        {
            GameObject.Destroy(_destination);
            GameObject.Destroy(_teleportMarker);
            agent.config.destination = _target.transform;
            _attacking = false;
            _teleportTimer = 0;
            _aStarAgent.Speed = _aStarAgent.Speed * 0.5f;
            _flyingController.delay = 2f;
        }
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
