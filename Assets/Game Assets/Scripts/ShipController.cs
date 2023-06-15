using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    //private bool _inShipState;

    [SerializeField] private GameObject _waypoint;
    [SerializeField] private PlayerState _ps;
    [SerializeField] public Vector3 velocity;

    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _accelerationDelay;
    private float _speed = 0f;
    private float _accelerate;

    [SerializeField] private InputHandler _input;

    private IEnumerator _accelerator;
    

    // Update is called once per frame
    void Update()
    {

        if (_waypoint)
        {
            Debug.Log(_speed);
            transform.position = Vector3.MoveTowards(transform.position, _waypoint.transform.position, _speed * Time.deltaTime);
            
            //NOTE: Change Velocity.z to apply to Velocity.x as well. How would we set the Vector3 Velocity to the same as the ships?
            velocity.z = _speed * Time.deltaTime;
        }

        ControlShip();
    }

    private void ControlShip()
    {
        //if the player is in ship state
       if(_ps.currentState.ToString().Equals("Ship"))
       {
            // if the player is trying to rotate the ship
            if(_input.move.x > 0)
                transform.Rotate(0f, .5f, 0f);
            else if(_input.move.x < 0)
                transform.Rotate(0f, -.5f, 0f);

            // if the player is trying to move the ship forward
            if(_input.move.y > 0)
            {
                if(_speed < _maxSpeed && Time.time > _accelerate)
                {
                    _speed = (_speed * 2) + .05f;

                    if (_speed > _maxSpeed)
                        _speed = _maxSpeed;

                    _accelerate = Time.time + _accelerationDelay;
                }
            }
            //if the player is not moving the ship forwards
            else if (_input.move.y < 0)
            {
                if(_speed > 0 && Time.time > _accelerate)
                {
                    _speed = (_speed / 2) - .05f;

                    if (_speed < 0)
                        _speed = 0;

                    _accelerate = Time.time + _accelerationDelay;
                }
            }
                
        }
    }
}
