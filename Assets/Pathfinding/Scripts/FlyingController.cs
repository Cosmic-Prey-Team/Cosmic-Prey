using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour
{
    AIAgent aiAgent;
    AStarAgent _Agent;
    [SerializeField] Vector3 target;
    //[SerializeField] Animator _Anim;
    //[SerializeField] AnimationCurve _SpeedCurve;
    //[SerializeField] float _Speed;
    private void Start()
    {
        aiAgent = GetComponent<AIAgent>();
        _Agent = GetComponent<AStarAgent>();
        StartCoroutine(Coroutine_MoveRandom());
    }

    IEnumerator Coroutine_MoveRandom()
    {      
            
        List<Point> freePoints = WorldManager.Instance.GetFreePoints();
        Point start = freePoints[Random.Range(0, freePoints.Count)];
        transform.position = start.WorldPosition;
        while (aiAgent.config.destination == null)
        {
            yield return null;
            Debug.Log(aiAgent.config.destination);
        }
        target = aiAgent.config.destination.position;
        _Agent.Pathfinding(aiAgent.config.destination.position);

        while (_Agent.Status != AStarAgentStatus.Finished)
            {

            yield return new WaitForSeconds(2f);
            if (target != aiAgent.config.destination.position)
            {
                target = aiAgent.config.destination.position;
                _Agent.Pathfinding(aiAgent.config.destination.position);
            }
        }
    }
    /**
    IEnumerator Coroutine_Animation()
    {
        _Anim.SetBool("Flying", true);
        while (_Agent.Status != AStarAgentStatus.Finished)
        {
            yield return null;
        }
        _Anim.SetBool("Flying", false);
    }
    **/
}
