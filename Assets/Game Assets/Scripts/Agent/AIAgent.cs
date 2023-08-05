using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public AIAgentConfig config;

    public GameObject _hitbox;
    [HideInInspector] public List<GameObject> krill;

   
    // Start is called before the first frame update
    void Start()
    {
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
