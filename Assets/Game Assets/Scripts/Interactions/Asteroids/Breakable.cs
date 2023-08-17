using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    [SerializeField] private GameObject _replacement; //broken asteroid
    //[SerializeField] private GameObject _original; //original asteroid

    [SerializeField] private float _collisionForce = 5f;
    [SerializeField] private float _collisionMultiplier = 50f;
    [SerializeField] private float _collisionRadius = 200f;
    [SerializeField] private int time = 5; //time taken to despawn
    [SerializeField] GameObject _smokeObject;

    public Vector3 explosionPoint;

    private bool _broken = false;

    //On BreakAsteroid the broken asteroid will replace the original one and a force will be applied to cause it to crumble
    // A smoke effect will also be applied at the same time
    // After a set period of time it will despawn
    public void BreakAsteroid()
    {
        //Debug.Log("BreakAsteroid()");
        if(_broken == false)
        {
            
            _broken = true;
            //spawns replacement in place of original
            var replacement = Instantiate(_replacement, null);
            //Debug.Log("rp: " + replacement.name);
            replacement.transform.position = transform.position;
            replacement.transform.rotation = transform.rotation;

            if(_smokeObject != null)
            {
                //VisualEffect _smokeEffect;



            }

            

            //calls each child
            var rbs = replacement.GetComponentsInChildren<Rigidbody>();
            //adds force to each child
            foreach (var rb in rbs)
            {
                if(Random.Range(0,2) == 0)
                {
                    _collisionForce = _collisionForce * 1;
                }else
                {
                    _collisionForce = _collisionForce * -1;
                }
                rb.transform.SetParent(null);
                rb.AddExplosionForce(_collisionForce * _collisionMultiplier, transform.position, _collisionRadius);
                //Delete object sequence
                Destroy(rb.gameObject, time);
            }

            Destroy(replacement.gameObject, time);
            Destroy(gameObject);
        }
    }

}
