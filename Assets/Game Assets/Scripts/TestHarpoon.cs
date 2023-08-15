using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHarpoon : MonoBehaviour
{
    private Rigidbody _rbody;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _rbody.velocity = transform.forward * 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
