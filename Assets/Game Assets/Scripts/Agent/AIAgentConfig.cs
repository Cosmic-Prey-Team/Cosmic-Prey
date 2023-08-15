using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    public string name;
    public AIStateID newState;
    public GameObject Waypoint;
    public LayerMask occlusionLayers;
    //public GameObject teleportEffect;
    public GameObject enemyPrefab;
    public Transform destination;
    public Animator animator;
    public int layerIndex;
    public float maxDistance;
    public float minDistance;
}
