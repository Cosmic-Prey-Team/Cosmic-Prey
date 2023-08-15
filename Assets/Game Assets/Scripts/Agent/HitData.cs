using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitData
{
    public int damage;
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public IHurtBox hurtBox;
    public IHitDetector hitDetector;

    public bool Validate()
    {
        if (hurtBox != null)
        {
            if (hurtBox.CheckHit(this))
            {
                if (hurtBox.HurtResponder == null || hurtBox.HurtResponder.CheckHit(this))
                {
                    if (hitDetector.HitResponder == null || hitDetector.HitResponder.CheckHit(this))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}

public interface IHitResponder
{
    int Damage { get; }
    public bool CheckHit(HitData data);
    public void Response(HitData data);
}

public interface IHitDetector
{
    public IHitResponder HitResponder { get; set; }
    public void CheckHit();
}

public interface IHurtResponder
{
    public bool CheckHit(HitData hitdata);
    public void Response(HitData data);
}
public interface IHurtBox
{
    public bool Active { get; }
    public GameObject Owner { get; }
    public Transform Transform { get; }
    public IHurtResponder HurtResponder { get; set; }
    public bool CheckHit(HitData data);
}