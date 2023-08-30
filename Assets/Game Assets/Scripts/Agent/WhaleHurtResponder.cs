using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleHurtResponder : MonoBehaviour, IHurtResponder
{
    private List<HurtBox> m_hurtboxes = new List<HurtBox>();
    public AIAgent _agent;

    private void Start()
    {
        m_hurtboxes = new List<HurtBox>(GetComponentsInChildren<HurtBox>());
        foreach (HurtBox _hurtBox in m_hurtboxes)
        {
            _hurtBox.HurtResponder = this;
        }
        _agent = gameObject.GetComponent<AIAgent>();
    }

    bool IHurtResponder.CheckHit(HitData hitdata)
    {
        return true;
    }

    void IHurtResponder.Response(HitData data) 
    {
        gameObject.GetComponent<Health>().Damage(data.damage);
        Debug.Log(_agent.stateMachine.currentState);
        if (_agent.stateMachine.currentState == AIStateID.WhaleWander)
        {
            _agent.stateMachine.ChangeState(AIStateID.WhaleFlee);
            Debug.Log(_agent.stateMachine.currentState + "second");
        }
    }
}
