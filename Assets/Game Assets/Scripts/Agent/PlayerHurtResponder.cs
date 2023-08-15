using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtResponder : MonoBehaviour, IHurtResponder
{
    private List<HurtBox> m_hurtboxes = new List<HurtBox>();

    private void Start()
    {
        m_hurtboxes = new List<HurtBox>(GetComponentsInChildren<HurtBox>());
        foreach (HurtBox _hurtBox in m_hurtboxes)
        {
            _hurtBox.HurtResponder = this;
        }
    }

    bool IHurtResponder.CheckHit(HitData hitdata)
    {
        return true;
    }

    void IHurtResponder.Response(HitData data) 
    {
        gameObject.GetComponent<Health>().Damage(data.damage);
    }
}
