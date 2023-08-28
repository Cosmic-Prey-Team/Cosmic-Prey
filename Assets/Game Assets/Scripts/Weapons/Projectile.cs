using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{

    [SerializeField] GameObject _hitbox;
    [SerializeField] float _speed;
    [SerializeField] float _lifetime = 5f;

    private DestroyHitResponder _hitResponder;
    private int _damage;
    private Rigidbody _rbody;
    private Transform _origin;


    public void Configure(Transform originTransform, int damage)
    {
        //configure
        _hitResponder = _hitbox.GetComponent<DestroyHitResponder>();
        _origin = originTransform;
        //_damage = damage;

        transform.rotation = _origin.rotation;

        //start
        _rbody = GetComponent<Rigidbody>();
        Destroy(gameObject, _lifetime);

        _rbody.velocity = _origin.forward * _speed;
    }

    public void Update()
    {
        _hitResponder._hitBox.CheckHit();
    }
   
}
