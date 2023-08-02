using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

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
            while (true)
            {
                //This is terrible but it works and can adjust distance to keep from player with stop radius on follow path. Make this not repath every time and also end
                //This also just doesnt work since the ship will be moving and this will not path accordingly
                _nodes[0] = aiAgent.config.destination.position;
                aiAgent.path = new LinePath(_nodes);
                aiAgent.path.CalcDistances();
                Vector3 accel = aiAgent.followPath.GetSteering(aiAgent.path);
                

                aiAgent.steeringBasics.Steer(accel);
                Vector3 forwardDirection = (_nodes[0] - transform.position).normalized;
                transform.forward = forwardDirection;
                yield return new WaitForSeconds(delay);
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
