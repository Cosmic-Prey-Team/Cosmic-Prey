using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private GameObject _waypoint;
    [SerializeField] private GameObject _verticalWaypoint;
    [SerializeField] private PlayerState _ps;
    [SerializeField] public Vector3 velocity;
    [SerializeField] private Vector3 rotate;
    [SerializeField] public float rotateSpeed = .5f;

    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _accelerationDelay;
    private float _speed, _vSpeed = 0f;
    private float _accelerate, _vAccelerate;
    public bool rotating;

    [SerializeField] private InputHandler _input;
    

    // Update is called once per frame
    void Update()
    {

        if (_waypoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, _waypoint.transform.position, _speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _verticalWaypoint.transform.position, _vSpeed * Time.deltaTime);
            
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
            if (_input.move.x > 0)
            {
                rotating = true;
                if (rotateSpeed < 0)
                    rotateSpeed = -rotateSpeed;
                rotate = new Vector3(0f, rotateSpeed, 0f);
                transform.Rotate(rotate);
            }    
            else if(_input.move.x < 0)
            {
                rotating = true;
                if (rotateSpeed > 0)
                    rotateSpeed = -rotateSpeed;
                rotate = new Vector3(0f, rotateSpeed, 0f);
                transform.Rotate(rotate);
            }
            else
            {
                rotating = false;
            }
               

            // if the player is trying to move the ship forward
            if(_input.move.y > 0)
            {
                if(_speed < _maxSpeed && Time.time > _accelerate)
                {
                    _speed = (_speed * 2) + .05f;

                    if (_speed > _maxSpeed)
                        _speed = _maxSpeed;

                    _accelerate = Time.time + _accelerationDelay;
                    velocity = -transform.forward * _speed * Time.deltaTime;
                }
            }
            //if the player is trying to stop moving
            else if (_input.move.y < 0)
            {
                if(_speed > 0 && Time.time > _accelerate)
                {
                    _speed = (_speed / 2) - .05f;

                    if (_speed < 0)
                        _speed = 0;

                    velocity = -transform.forward *_speed * Time.deltaTime;
                }
            }
            
            //if the player is trying to move up
            if (_input.vMove > 0)
            {
                if(_vSpeed < _maxSpeed && Time.time > _vAccelerate)
                {
                    if (_vSpeed >= 0)
                        _vSpeed = (_vSpeed * 2) + .05f;
                    else
                        _vSpeed = (_vSpeed / 2) + .05f;

                    if (_vSpeed > _maxSpeed)
                        _vSpeed = _maxSpeed;

                    _vAccelerate = Time.time + _accelerationDelay;
                    velocity.y = _vSpeed * Time.deltaTime;
                }
            }
            //if the player is trying to move down
            else if (_input.vMove < 0)
            {
                if (_vSpeed > -_maxSpeed && Time.time > _vAccelerate)
                {
                    if (_vSpeed <= 0)
                        _vSpeed = (_vSpeed * 2) - .05f;
                    else
                        _vSpeed = (_vSpeed / 2) - .05f;

                    if (_vSpeed < -_maxSpeed)
                        _vSpeed = -_maxSpeed;

                    _vAccelerate = Time.time + _accelerationDelay;
                    velocity.y = _vSpeed * Time.deltaTime;
                }
            }
        }

       //bring vertical movement to 0 if there is no input or player is no longer controlling ship
       if(_input.vMove == 0)
        {
            if (_vSpeed > 0 && Time.time > _vAccelerate)
            {
                _vSpeed = (_vSpeed / 2) - .05f;

                if (_vSpeed < 0)
                    _vSpeed = 0;

                _vAccelerate = Time.time + _accelerationDelay;
                velocity.y = _vSpeed * Time.deltaTime;
            }
            else if(_vSpeed < 0 && Time.time > _vAccelerate)
            {
                _vSpeed = (_vSpeed / 2) + .05f;

                if (_vSpeed > 0)
                    _vSpeed = 0;

                _vAccelerate = Time.time + _accelerationDelay;
                velocity.y = _vSpeed * Time.deltaTime;
            }
        }
    }
}
