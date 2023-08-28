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
    private Vector3 _lastPos;
    //[SerializeField] Animator _Anim;
    //[SerializeField] AnimationCurve _SpeedCurve;
    //[SerializeField] float _Speed;
    private void Start()
    {
        aiAgent = GetComponent<AIAgent>();
        _Agent = GetComponent<AStarAgent>();
        StartCoroutine(Restart_Coroutine_MoveRandom());
    }

    IEnumerator Coroutine_MoveRandom()
    {
        //List<Point> freePoints = WorldManager.Instance.GetFreePoints();
        //Point start = freePoints[Random.Range(0, freePoints.Count)];
        //transform.position = start.WorldPosition;
        while (true)
        {
            while (aiAgent.config.destination == null)
            {
                Debug.Log("null loop");
                yield return null;
            }
            target = aiAgent.config.destination.position;
            _Agent.Pathfinding(aiAgent.config.destination.position);

            while (_Agent.Status != AStarAgentStatus.Finished && _Agent.Status != AStarAgentStatus.Invalid)
            {
                yield return new WaitForSeconds(delay);
                Debug.Log(aiAgent.config.destination.position + "" + aiAgent.config.destination);
                if (target != aiAgent.config.destination.position || transform.position == _lastPos)
                {
                    Debug.Log("Repath" + (transform.position == _lastPos));
                    target = aiAgent.config.destination.position;
                    _Agent.Pathfinding(aiAgent.config.destination.position);
                }
                _lastPos = transform.position;

            }
            Debug.Log("finished");
            while ((aiAgent.config.destination.position - transform.position).magnitude < aiAgent.config.maxDistance)
            {
                Debug.Log("finished loop" + aiAgent.config.destination.position + "" + aiAgent.config.destination);
                transform.forward = Vector3.Slerp(transform.forward, (aiAgent.config.destination.position - transform.position).normalized, Time.deltaTime * _Agent.TurnSpeed); //* 2);
                if ((aiAgent.config.destination.position - transform.position).magnitude > aiAgent.config.minDistance)
                {
                    //transform.forward = (aiAgent.config.destination.position - transform.position).normalized;
                    transform.position = Vector3.MoveTowards(transform.position, aiAgent.config.destination.position, Time.deltaTime * _Agent.Speed);
                    //transform.position += transform.forward * Time.deltaTime * _Agent.Speed;
                    //transform.forward = Vector3.Slerp(transform.forward, (aiAgent.config.destination.position - transform.position).normalized, Time.deltaTime * _Agent.TurnSpeed*2);
                }
                yield return null;


            }

            if (aiAgent.stateMachine.currentState == AIStateID.KrillAttack && _Agent.Status == AStarAgentStatus.Invalid || _Agent.Status == AStarAgentStatus.Finished)
            {
                Debug.Log("Forcing Attack Pathing");
                transform.forward = Vector3.Slerp(transform.forward, (aiAgent.config.destination.position - transform.position).normalized, Time.deltaTime * _Agent.TurnSpeed * 2);
                transform.position = Vector3.MoveTowards(transform.position, aiAgent.config.destination.position, Time.deltaTime * _Agent.Speed);
            }
            Debug.Log("Dead?" + aiAgent.config.destination.position + "   " + _Agent.Status + "    " + gameObject);
            yield return null;
        }
    }

    IEnumerator Restart_Coroutine_MoveRandom()
    {
        Debug.Log("restarting");
        yield return StartCoroutine(Coroutine_MoveRandom());
        Debug.Log("ended routine");
    }
}