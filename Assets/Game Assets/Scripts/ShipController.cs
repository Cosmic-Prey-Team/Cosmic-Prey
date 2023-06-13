using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private bool _inShipState = false;

    [SerializeField] private GameObject _waypoint;

    //private int _currentWaypointIndex = 0;

    [SerializeField] private float _speed = 1f;
    public void ChangeShipState()
    {
        if (_inShipState)
            _inShipState = false;
        else
            _inShipState = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypoint.transform.position, _speed * Time.deltaTime);
    }
}
