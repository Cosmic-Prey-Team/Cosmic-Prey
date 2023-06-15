using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private bool _inShipState = false;

    [SerializeField] private GameObject _waypoint;
    [SerializeField] public Vector3 velocity;

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
        //if (rbShip)
        //    rbShip.AddForce(0, 0, 1f);

        if (_waypoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, _waypoint.transform.position, _speed * Time.deltaTime);
            
            //NOTE: Change Velocity.z to apply to Velocity.x as well. How would we set the Vector3 Velocity to the same as the ships?
            velocity.z = _speed * Time.deltaTime;
        }
            

    }
}
