using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarAgent))]
public class CharacterMoveToPoint : MonoBehaviour
{
    AStarAgent _Agent;
    [SerializeField] Transform targetPoint;

    private void Start()
    {
        _Agent = GetComponent<AStarAgent>();
        StartCoroutine(Coroutine_MoveAB());
    }

    IEnumerator Coroutine_MoveAB()
    {
        yield return null;
        while (true)
        {
            _Agent.Pathfinding(targetPoint.position);
            while (_Agent.Status == AStarAgentStatus.Invalid)
            {
                Debug.Log("dead");
             
                _Agent.Pathfinding(targetPoint.position);
                yield return new WaitForSeconds(0.2f);
            }
            while (_Agent.Status != AStarAgentStatus.Finished)
            {
                yield return new WaitForSeconds(0.2f);
                yield return null;
            }
            yield return null;
            Debug.Log("dead2");
            
        }
    }
}
