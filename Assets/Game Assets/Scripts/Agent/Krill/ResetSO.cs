using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

public class ResetSO : MonoBehaviour
{
    public AIAgentConfig config;
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR
        config.krill = new List<MovementAIRigidbody>();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
