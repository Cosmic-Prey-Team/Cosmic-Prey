using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHitResponder : MonoBehaviour, IHitResponder
{
    [SerializeField] private int _damage = 10;
    [SerializeField] public HitBox _hitBox;
    public List<GameObject> _objectsHit = new List<GameObject>();
    public string targetTag = null;

    int IHitResponder.Damage { get => _damage; }
    // Start is called before the first frame update
    void Start()
    {
        _hitBox.HitResponder = this;
    }

    //Should be called whenever attack
    bool IHitResponder.CheckHit(HitData data)
    {
        if (_objectsHit.Contains(data.hurtBox.Owner))
        {
            return false;
        }
        if (targetTag != null && !data.hurtBox.Owner.CompareTag(targetTag))
        {
            Debug.Log("Bad");
            return false;
        }
        Debug.Log("Check done");
        return true; 
    }
    void IHitResponder.Response(HitData data)
    {
        _objectsHit.Add(data.hurtBox.Owner);
    }
}
