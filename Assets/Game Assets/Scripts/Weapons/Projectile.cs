using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _lifetime = 5f;

    private int _damage;
    private Rigidbody _rbody;
    private Transform _origin;

    private void Start()
    {
        


    }
    public void Configure(Transform originTransform, int damage)
    {
        //configure
        _origin = originTransform;
        _damage = damage;

        transform.rotation = _origin.rotation;

        //start
        _rbody = GetComponent<Rigidbody>();
        Destroy(gameObject, _lifetime);

        _rbody.velocity = _origin.forward * _speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
