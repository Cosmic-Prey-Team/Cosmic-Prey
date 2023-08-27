using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrillFlyingController : MonoBehaviour
{
    AIAgent aiAgent;
    AStarAgent _Agent;
    [SerializeField] Vector3 target;
    [HideInInspector] 
    public float delay = 2f;
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
        while (true)
        {
            while (aiAgent.config.destination == null)
            {
                yield return null;
            }
            target = aiAgent.config.destination.position;
            _Agent.Pathfinding(aiAgent.config.destination.position);

            while (_Agent.Status != AStarAgentStatus.Finished && _Agent.Status != AStarAgentStatus.Invalid)
            {
                yield return new WaitForSeconds(delay);
                if (target != aiAgent.config.destination.position)
                {
                    target = aiAgent.config.destination.position;
                    _Agent.Pathfinding(aiAgent.config.destination.position);
                }
            }           
            Debug.Log("finished");
            
            while ((aiAgent.config.destination.position - transform.position).magnitude < aiAgent.config.maxDistance)
            {
                transform.forward = Vector3.Slerp(transform.forward, (aiAgent.config.destination.position - transform.position).normalized, Time.deltaTime * _Agent.TurnSpeed * 2);
                if ((aiAgent.config.destination.position - transform.position).magnitude > aiAgent.config.minDistance)
                {
                    //transform.forward = (aiAgent.config.destination.position - transform.position).normalized;
                    transform.position = Vector3.MoveTowards(transform.position, aiAgent.config.destination.position, Time.deltaTime * _Agent.Speed);
                    //transform.position += transform.forward * Time.deltaTime * _Agent.Speed;
                    //transform.forward = Vector3.Slerp(transform.forward, (aiAgent.config.destination.position - transform.position).normalized, Time.deltaTime * _Agent.TurnSpeed*2);
                }
                yield return null;


            }
            if (aiAgent.stateMachine.currentState == AIStateID.KrillAttack && _Agent.Status == AStarAgentStatus.Invalid)
            {
                Debug.Log("Forcing Attack Pathing");
                transform.forward = Vector3.Slerp(transform.forward, (aiAgent.config.destination.position - transform.position).normalized, Time.deltaTime * _Agent.TurnSpeed * 2);
                transform.position = Vector3.MoveTowards(transform.position, aiAgent.config.destination.position, Time.deltaTime * _Agent.Speed);
            }
            Debug.Log("Dead?" + aiAgent.config.destination.position + "   " + _Agent.Status + "    " + gameObject);
            yield return null;
        }
    }
    
}