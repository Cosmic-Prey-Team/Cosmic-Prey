using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public AIAgentConfig config;

    public LinePath path;

    [HideInInspector]
    public SteeringBasics steeringBasics;
    [HideInInspector]
    public WallAvoidance wallAvoidance;
    [HideInInspector]
    public FollowPath followPath;

   
    // Start is called before the first frame update
    void Start()
    {
        

        steeringBasics = GetComponent<SteeringBasics>();
        wallAvoidance = GetComponent<WallAvoidance>();
        followPath = GetComponent<FollowPath>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIWhaleWanderState());
        stateMachine.RegisterState(new AIWhaleIdleState());
        stateMachine.RegisterState(new AIWhaleAttackState());
        stateMachine.RegisterState(new AIWhaleFleeState());
        stateMachine.RegisterState(new AIKrillFollowState());
        stateMachine.RegisterState(new AIKrillAttackState());        
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
