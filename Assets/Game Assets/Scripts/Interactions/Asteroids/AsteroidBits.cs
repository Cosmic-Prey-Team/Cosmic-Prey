using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBits : MonoBehaviour
{
    [SerializeField] private float _collisionForce = 5f;
    [SerializeField] private float _collisionMultiplier = 50f;
    [SerializeField] private float _collisionRadius = 2f;
    [SerializeField] private int time = 5; //time taken to despawn


    private void Start()
    {
        //calls each child
        var rbs = GetComponentsInChildren<Rigidbody>();
        //adds force to each child
        foreach (var rb in rbs)
        {
            rb.transform.SetParent(null);
            rb.AddExplosionForce(_collisionForce * _collisionMultiplier, transform.position, _collisionRadius);
            //Delete object sequence
            Destroy(rb.gameObject, time);
        }
    }
}
