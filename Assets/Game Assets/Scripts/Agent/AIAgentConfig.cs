using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{    
    public float Speed;
    public GameObject Waypoint;
    public LayerMask occlusionLayers;
    public GameObject teleportEffect;
    public GameObject enemyPrefab;
}
