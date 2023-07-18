using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermWaypoint : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Whale"))
        {
            AIAgent agent = other.GetComponent<AIAgent>();
            //AIWhaleIdleState idleState = agent.stateMachine.GetState(AIStateID.WhaleIdle) as AIWhaleIdleState;
            agent.stateMachine.ChangeState(AIStateID.WhaleAttack);
        }
    }
}
