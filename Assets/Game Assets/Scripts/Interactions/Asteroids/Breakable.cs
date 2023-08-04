using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    [SerializeField] private GameObject _replacement; //broken asteroid
    //[SerializeField] private GameObject _original; //original asteroid

    public Vector3 explosionPoint;

    //On awake the broken asteroid will replace the original one and a force will be applied to cause it to crumble
    // A smoke effect will also be applied at the same time
    // After a set period of time it will despawn
    void Awake()
    {

    }

    public void AlmostAwake()
    {
        //get position of drill, so that's where the force comes from
        //Vector3 mousePos = Input.mousePosition;

        //spawns replacement in place of original
        var replacement = Instantiate(_replacement, null);
                
        //Debug.Log("asteroid: " + (transform.TransformPoint(transform.position) / 3.2f));
        //Debug.Log("explosion poinnt: " + explosionPoint);   //_original.transform.position;
        replacement.transform.position = transform.position;
        replacement.transform.rotation = transform.rotation;

        
    }

}
