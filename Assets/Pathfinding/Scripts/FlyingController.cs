using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour
{
    AIAgent aiAgent;
    AStarAgent _Agent;
    [SerializeField] Vector3 target;
    [HideInInspector] 
    public float delay = 2f;
    private Vector3[] _nodes = new Vector3[1];
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
                Debug.Log(aiAgent.config.destination);
            }
            target = aiAgent.config.destination.position;
            _Agent.Pathfinding(aiAgent.config.destination.position);

            while (_Agent.Status != AStarAgentStatus.Finished && _Agent.Status != AStarAgentStatus.Invalid)
            {
                Debug.Log("still going");
                yield return new WaitForSeconds(delay);
                if (target != aiAgent.config.destination.position)
                {
                    Debug.Log("repath 1");
                    target = aiAgent.config.destination.position;
                    _Agent.Pathfinding(aiAgent.config.destination.position);
                }
                else if ((aiAgent.config.destination.position - gameObject.transform.position).magnitude < 15)
                {
                    Debug.Log("repath 2");
                    //target = aiAgent.config.destination.position;
                    //_Agent.Pathfinding(aiAgent.config.destination.position);
                }
            }           
            Debug.Log("finished");
            
            while ((aiAgent.config.destination.position - transform.position).magnitude < 12)
            {
                if ((aiAgent.config.destination.position - transform.position).magnitude > 1.5)
                {
                    transform.forward = (aiAgent.config.destination.position - transform.position).normalized;
                    transform.position = Vector3.MoveTowards(transform.position, aiAgent.config.destination.position, Time.deltaTime * _Agent.Speed);
                    //transform.forward = Vector3.Slerp(transform.forward, forwardDirection, Time.deltaTime * TurnSpeed);
                }
                yield return null;


            }
            
        }
    }
    
}
