using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    [SerializeField] private GameObject _replacement;
    [SerializeField] private float _breakForce = 2;
    [SerializeField] private float _collisionMultiplier = 100;
    [SerializeField] private bool _broken;
    [SerializeField] private int relativeVelocity;
    void Awake()
    {
        /*var replacement = Instantiate(_replacement, transform.position, transform.rotation);

        private var rbs = replacement.GetComponentInChildren<Rigidbody>();
        foreach(var rb in rbs)
        {
            rb.AddExplosionForce(relativeVelocity * _collisionMultiplier, 1, 2);
        }*/
    }
}
