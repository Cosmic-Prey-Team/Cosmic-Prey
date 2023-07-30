using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{    
    public float Speed;
    public GameObject Waypoint;
    public LayerMask occlusionLayers;
    public GameObject teleportEffect;
    public GameObject enemyPrefab;
    public List<MovementAIRigidbody> krill;
    public Transform destination;
    public Animator animator;
    public int layerIndex;
}
