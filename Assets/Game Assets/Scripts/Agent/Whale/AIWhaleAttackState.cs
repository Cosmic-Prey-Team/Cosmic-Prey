using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AIWhaleAttackState : AIState
{
    private GameObject _teleportMarker = null;
    private Transform _destination = null;
    private GameObject _target;
    private float _aggroTimer = 0f;
    private float _chargeTimer = 0f;
    private float _teleportTimer = 0f;
    private float _spawnTimer = 0f;
    private float _updateTimer = 0f;
    private float _teleportCooldown = 15f;
    private float _spawnCooldown = 20f;
    protected bool _attacking = false;
    protected bool _charging = false;
    protected int _attack;
    private WhaleFlyingController _flyingController;
    private AStarAgent _aStarAgent;
    private AISensor _sensor;
    private Animator _animator;
    private BasicHitResponder _hitResponder;



    public void Enter(AIAgent agent)
    {
        Debug.Log("Attack");
        _sensor = agent.GetComponent<AISensor>();
        _target = GameObject.FindGameObjectWithTag("Ship");
        _flyingController = agent.GetComponent<WhaleFlyingController>();
        _aStarAgent = agent.GetComponent<AStarAgent>();
        agent.config.destination = _target.transform;
        _animator = agent.GetComponentInChildren<Animator>();
        _hitResponder = agent._hitbox.GetComponent<BasicHitResponder>();
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

        if (!_charging) //|| _teleporting)
        {
            _attack = Random.Range(0, 3);
            Debug.Log(_attack);
        }

        if (_attack == 2 && _spawnTimer >= _spawnCooldown)
        {
            agent.krill.RemoveAll(k => k == null);
            if (agent.krill.Count < 5)
            {
                SpawnEnemies(agent);
            }           
            _spawnTimer = 0f;
        }
        else
        {
            Charge(agent);
        }
        /**
        else if (_attack == 1 && _teleportTimer >= _teleportCooldown)
        {
            Teleport(agent);
        }
        **/

        /**
        if (_teleportTimer < _teleportCooldown)
        {
            _teleportTimer += Time.deltaTime;
        }
        **/
        if (_spawnTimer < _spawnCooldown)
        {
            _spawnTimer += Time.deltaTime;
        }       

        if ((_aggroTimer += Time.deltaTime) > 60f)
        {
            agent.stateMachine.ChangeState(AIStateID.WhaleFlee);
        }

    }

    private void Charge(AIAgent agent)
    {
        if (_charging == false)
        {
            _sensor.distance = 15f;
            _sensor.angle = 45;
            _aStarAgent.Speed = _aStarAgent.Speed * 2;
            agent.config.destination = _target.transform;
            _flyingController.delay = 0.33f;
        }

        _charging = true;

        agent.config.destination = _target.transform;
        _chargeTimer += Time.deltaTime;
        _updateTimer += Time.deltaTime;

        if (_chargeTimer > 5)
        {
            _charging = false;
            _chargeTimer = 0;
            _sensor.distance = 40f;
            _sensor.angle = 90;
            _aStarAgent.Speed = _aStarAgent.Speed * 0.5f;
            _flyingController.delay = 2f;
        }

        AttemptAttack();
    }

    /**
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
    **/

    private void SpawnEnemies(AIAgent agent)
    {
        for (int i = 0; i < Random.Range(4,6); i++)
        {
            Vector3 offset = new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), Random.Range(-5, 6));
            agent.krill.Add(GameObject.Instantiate(agent.config.enemyPrefab, agent.gameObject.transform.position + offset, Quaternion.identity));
        }
        return;
    }

    public void AttemptAttack()
    {
        //This may cause it to play the attack animation while death animation plays
        int id = Animator.StringToHash("Stella_Headbutt_Final");
        if (_animator.HasState(0, id))
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.fullPathHash == id || state.shortNameHash == id)
            {
                _attacking = true;
                int totalFrames = GetTotalFrames(_animator, 0);

                int currentFrame = GetCurrentFrame(totalFrames, GetNormalizedTime(state));
                if (currentFrame > 64 && currentFrame < 90)
                {
                    _hitResponder._hitBox.CheckHit();
                }
                return;
            }
            _attacking = false;
        }
        if (_updateTimer > 0.33f)
        {
            _sensor.Scan();
            GameObject[] player = _sensor.Filter(new GameObject[1], "Ship");
            if (player[0] != null)
            {
                _animator.Play("Stella_Headbutt_Final", 0, 0.0f);
                _hitResponder._objectsHit = new List<GameObject>();
            }
            _updateTimer = 0;
        }
    }

    private int GetTotalFrames(Animator animator, int layerIndex)
    {
        AnimatorClipInfo[] _clipInfos = animator.GetNextAnimatorClipInfo(layerIndex);
        if (_clipInfos.Length == 0)
        {
            _clipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);
        }

        AnimationClip clip = _clipInfos[0].clip;
        return Mathf.RoundToInt(clip.length * clip.frameRate);
    }

    private float GetNormalizedTime(AnimatorStateInfo stateInfo)
    {
        return stateInfo.normalizedTime > 1 ? 1 : stateInfo.normalizedTime;
    }

    private int GetCurrentFrame(int totalFrames, float normalizedTime)
    {
        return Mathf.RoundToInt(totalFrames * normalizedTime);
    }

}
