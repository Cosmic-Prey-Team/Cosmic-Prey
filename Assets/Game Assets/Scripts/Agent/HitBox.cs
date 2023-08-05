using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour, IHitDetector
{
    [SerializeField] private BoxCollider m_collider;
    [SerializeField] private LayerMask m_layerMask;

    private float m_thickness = 0.025f;
    private IHitResponder m_hitResponder;
    public IHitResponder HitResponder { get => m_hitResponder; set => m_hitResponder = value; }

    public void CheckHit()
    {
        Vector3 _scaledSize = new Vector3(
            m_collider.size.x * transform.lossyScale.x,
            m_collider.size.y * transform.lossyScale.y,
            m_collider.size.z * transform.lossyScale.z
            );

        float _distance = _scaledSize.y - m_thickness;
        Vector3 _direction = transform.up;
        Vector3 _center = transform.TransformPoint(m_collider.center);
        Vector3 _start = _center - _direction * (_distance / 2);
        Vector3 _halfExtents = new Vector3(_scaledSize.z, m_thickness, _scaledSize.z) / 2;
        Quaternion _orientation = transform.rotation;

        HitData _hitData = null;
        IHurtBox _hurtBox = null;
        //Registers all hits instead of just first one
        RaycastHit[] _hits = Physics.BoxCastAll(_start, _halfExtents, _direction, _orientation, _distance, m_layerMask);
        foreach (RaycastHit _hit in _hits)
        {
            _hurtBox = _hit.collider.GetComponent<IHurtBox>();
            if (_hurtBox != null && _hurtBox.Active)
            {
                _hitData = new HitData
                {
                    damage = m_hitResponder == null ? 0 : m_hitResponder.Damage,
                    hitPoint = _hit.point == Vector3.zero ? _center : _hit.point,
                    hitNormal = _hit.normal,
                    hurtBox = _hurtBox,
                    hitDetector = this
                };

                if (_hitData.Validate())
                {
                    _hitData.hitDetector.HitResponder?.Response(_hitData);
                    _hitData.hurtBox.HurtResponder?.Response(_hitData);
                }
            }
        }
    }
}
