using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;


public class AIKrillAttackState : AIState
{
       
    private GameObject _target;
    private float _chargeTimer = 0f;  
    private float _updateTimer = 0f;
    protected bool _attacking = false;
    protected bool _charging = false;
    protected int _attack;
    private int _numAttacks = 0;
    private KrillFlyingController _flyingController;
    private AStarAgent _aStarAgent;
    private AISensor _sensor;
    private Animator _animator;
    private BasicHitResponder _hitResponder;
    private WorldManager worldManager;
    private GameObject _delayTarget;


    public void Enter(AIAgent agent)
    {
        _sensor = agent.GetComponent<AISensor>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _flyingController = agent.GetComponent<KrillFlyingController>();
        _aStarAgent = agent.GetComponent<AStarAgent>();
        _animator = agent.GetComponent<Animator>();
        _hitResponder = agent._hitbox.GetComponent<BasicHitResponder>();
        worldManager = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
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

        if (_delayTarget == null)
        {
            //Debug.Log(_numAttacks + " " + _charging);
            if (_charging == false && _numAttacks + UnityEngine.Random.Range(0, 2) > 2)
            {
                Vector3 offset = new Vector3(UnityEngine.Random.Range(-10, 11), UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(-10, 11));
                Vector3 destination = worldManager.GetClosestPointWorldSpace(agent.transform.position + offset).WorldPosition;
                _delayTarget = GameObject.Instantiate(agent.config.Waypoint, destination, Quaternion.identity);
                agent.config.destination = _delayTarget.transform;
                _numAttacks = 0;
            }
            else
            {
                Charge(agent);
            }                             
        }


    }

    private void Charge(AIAgent agent)
    {
        if (_charging == false)
        {
            _sensor.distance = 2f;
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
            _sensor.distance = 7f;
            _sensor.angle = 90;
            _aStarAgent.Speed = _aStarAgent.Speed * 0.5f;
            _flyingController.delay = 2f;
        }

        AttemptAttack();
        
    }

    public void AttemptAttack()
    {
        //This may cause it to play the attack animation while death animation plays
        int id = Animator.StringToHash("Attack");
        if (_animator.HasState(0, id))
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.fullPathHash == id || state.shortNameHash == id)
            {
                _attacking = true;
                int totalFrames = GetTotalFrames(_animator, 0);

                int currentFrame = GetCurrentFrame(totalFrames, GetNormalizedTime(state));
                if (currentFrame > 24 && currentFrame < 36)
                {
                    //Krill can damage the ship
                    _hitResponder._hitBox.CheckHit();
                }                
                return;
            }
            _attacking = false;
        }
        if (_updateTimer > 0.33f)
        {
            _sensor.Scan();
            GameObject[] player = _sensor.Filter(new GameObject[1], "Player");
            if (player[0] != null)
            {
                _animator.Play("Attack", 0, 0.0f);
                _numAttacks++;
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
