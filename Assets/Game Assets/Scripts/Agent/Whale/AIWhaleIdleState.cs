using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AIWhaleIdleState : AIState
{
    
    public void Enter(AIAgent agent)
    {
        Debug.Log("Done.");
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AIStateID GetID()
    {
        return AIStateID.WhaleIdle;
    }

    public void Update(AIAgent agent)
    {
        
    }
}
