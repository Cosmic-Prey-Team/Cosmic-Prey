using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SMBEvent : AIState
{
    public enum SMBTiming { OnEnter, OnExit, OnUpdate, OnEnd};

    [System.Serializable]
    public class SMB_Event
    {
        public bool fired;
        public string eventName;
        public SMBTiming timing;
        public float onUpdateFrame;
    }

    [SerializeField] private int totalFrames;
    [SerializeField] private int currentFrame;
    [SerializeField] private float normalizedTime;
    [SerializeField] private float normalizedTimeUncapped;
    [SerializeField] private string motionTime = "";

    public List<SMB_Event> Events = new List<SMB_Event>();

    private bool hasParam;
    private SMBEventCurator eventCurator;

    public void Enter(AIAgent agent)
    {
        hasParam = HasParameter(agent.config.animator, motionTime);
        eventCurator = agent.config.animator.GetComponent<SMBEventCurator>();
        totalFrames = GetTotalFrames(agent.config.animator, agent.config.layerIndex);

        //normalizedTimeUncapped = stateInfo.normalizedTime;
        //normalizedTime = hasParam ? agent.config.animator.GetFloat(motionTime) : GetNormalizedTime(stateInfo);
        currentFrame = GetCurrentFrame(totalFrames, normalizedTime);

        if (eventCurator != null)
        {
            foreach (SMB_Event smbEvent in Events)
            {
                smbEvent.fired = false;
                if (smbEvent.timing == SMBTiming.OnEnter)
                {
                    smbEvent.fired = true;
                    eventCurator.Event.Invoke(smbEvent.eventName);
                }
            }
        }
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AIStateID GetID()
    {
        return AIStateID.WhaleWander;
    }

    public void Update(AIAgent agent)
    {
        //normalizedTimeUncapped = stateInfo.normalizedTime;
        //normalizedTime = hasParam ? agent.config.animator.GetFloat(motionTime) : GetNormalizedTime(stateInfo);
        currentFrame = GetCurrentFrame(totalFrames, normalizedTime);

        if (eventCurator != null)
        {
            foreach (SMB_Event smbEvent in Events)
            {
                if (!smbEvent.fired)
                {
                    if (smbEvent.timing == SMBTiming.OnUpdate)
                    {
                        if (currentFrame >= smbEvent.onUpdateFrame)
                        {
                            smbEvent.fired = true;
                            eventCurator.Event.Invoke(smbEvent.eventName);
                        }
                    }
                    else if (smbEvent.timing == SMBTiming.OnEnd)
                    {
                        if (currentFrame >= totalFrames)
                        {
                            smbEvent.fired = true;
                            eventCurator.Event.Invoke(smbEvent.eventName);
                        }
                    }
                }
            }
        }
    }

    private bool HasParameter(Animator animator, string parameterName)
    {
        if (string.IsNullOrEmpty(parameterName) || string.IsNullOrWhiteSpace(parameterName))
        {
            return false;
        }

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == parameterName)
            {
                return true;
            }
        }

        return false;
    }

    private int GetTotalFrames(Animator animator, int layerIndex)
    {
        AnimatorClipInfo[] _clipInfos = animator.GetNextAnimatorClipInfo(layerIndex);
        if (_clipInfos.Length == 0)
        {
            _clipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);
        }

        AnimationClip clip = _clipInfos[0].clip;
        return Mathf.RoundToInt(clip.length * clip.frameRate);
    }

    private float GetNormalizedTime(AnimatorStateInfo stateInfo)
    {
        return stateInfo.normalizedTime > 1 ? 1 : stateInfo.normalizedTime;
    }

    private int GetCurrentFrame(int totalFrames, float normalizedTime)
    {
            return Mathf.RoundToInt(totalFrames * normalizedTime);
    }
}
