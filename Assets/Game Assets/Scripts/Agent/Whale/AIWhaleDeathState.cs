using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;


public class AIWhaleDeathState : AIState
{
       
    
    private Animator _animator;


    public void Enter(AIAgent agent)
    {
        //_animator = agent.GetComponent<Animator>();
        //_animator.Play("Death", 0, 0.0f);
    }

    public void Exit(AIAgent agent)
    {

    }

    public AIStateID GetID()
    {
        return AIStateID.WhaleDeath;
    }

    public void Update(AIAgent agent)
    {
        //WaitForDeath(agent);
    }

   

    public void WaitForDeath(AIAgent agent)
    {
        //This may cause it to play the attack animation while death animation plays
        int id = Animator.StringToHash("Death");
        if (_animator.HasState(0, id))
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            int totalFrames = GetTotalFrames(_animator, 0);
            int currentFrame = GetCurrentFrame(totalFrames, GetNormalizedTime(state));
            if (currentFrame > 41)
            {
                GameObject.Destroy(agent.gameObject, 0.1f);
            }                          
        }       
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
