using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private GameObject _waypoint;
    [SerializeField] private GameObject _verticalWaypoint;

    [Header("Ship Properties")]
    [SerializeField] public Vector3 velocity;
    [SerializeField] private Vector3 rotate;
    [SerializeField] public float rotateSpeed = .5f;
    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _accelerationDelay;
    [Space]
    public bool Rotating;

    [Header("Events")]
    public UnityEvent OnShipStartMove;
    public UnityEvent OnShipStopMove;

    private float _speed, _vSpeed = 0f;
    private float _accelerate, _vAccelerate;

    private PlayerState _playerState;
    private ShipInput _input;

    private bool _isMoving = false;

    [SerializeField] ParticleSystem _crashFx;

    private void Awake()
    {
        _playerState = FindObjectOfType<PlayerState>();
        _input = _playerState.GetComponent<ShipInput>();
    }
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

        #region Trigger Events
        if (_input.thrust != 0 || _input.turn != 0 || _input.vMove != 0)
        {
            if(_isMoving == false)
            {
                _isMoving = true;
                OnShipStartMove?.Invoke();
            }
        }
        if (_input.thrust == 0 && _input.turn == 0 && _input.vMove == 0)
        {
            if (_isMoving == true)
            {
                _isMoving = false;
                OnShipStopMove?.Invoke();
            }
        }
        #endregion
    }

    private void ControlShip()
    {
        // if the player is trying to rotate the ship
        if (_input.turn != 0)
        {
            Rotating = true;

            if (_input.turn > 0 && rotateSpeed < 0 || _input.turn < 0 && rotateSpeed > 0)
                rotateSpeed = -rotateSpeed;

            rotate = new Vector3(0f, rotateSpeed, 0f);
            transform.Rotate(rotate);
        }
        else
        {
            Rotating = false;
        }

        // if the player is trying to move the ship forward
        if (_input.thrust > 0)
        {
            if (_speed < _maxSpeed && Time.time > _accelerate)
            {
                _speed = (_speed * 2) + .05f;

                if (_speed > _maxSpeed)
                    _speed = _maxSpeed;

                _accelerate = Time.time + _accelerationDelay;
                velocity = -transform.forward * _speed * Time.deltaTime;
            }
        }
        //if the player is trying to stop moving
        else if (_input.thrust < 0)
        {
            if (_speed > 0 && Time.time > _accelerate)
            {
                _speed = (_speed / 2) - .05f;

                if (_speed < 0)
                    _speed = 0;

                velocity = -transform.forward * _speed * Time.deltaTime;
            }
        }

        /*if (_input.thrust != 0)
        {
            if (_speed < _maxSpeed && Time.time > _accelerate)
            {
                _speed = (_speed * 2) + 0.5f;

                if (_speed > _maxSpeed) _speed = _maxSpeed;

                _accelerate = Time.time + _accelerationDelay;
                velocity = transform.forward * _input.thrust * _speed * Time.deltaTime;
            }
            else if (_speed > 0 && Time.time > _accelerate)
            {
                _speed = (_speed * 0.5f) - 0.5f;

                if (_speed > _maxSpeed) _speed = _maxSpeed;

                _accelerate = Time.time + _accelerationDelay;
                velocity = transform.forward * _input.thrust * _speed * Time.deltaTime;
            }
        }*/

        //vertical movement stuff
        if (_input.vMove != 0)
        {
            //if the player is trying to move up
            if (_input.vMove > 0)
            {
                if (_vSpeed < _maxSpeed && Time.time > _vAccelerate)
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
        else
        {
            //bring vertical movement to 0 if there is no input or player is no longer controlling ship
            if (_vSpeed > 0 && Time.time > _vAccelerate)
            {
                _vSpeed = (_vSpeed / 2) - .05f;

                if (_vSpeed < 0)
                    _vSpeed = 0;

                _vAccelerate = Time.time + _accelerationDelay;
                velocity.y = _vSpeed * Time.deltaTime;
            }
            else if (_vSpeed < 0 && Time.time > _vAccelerate)
            {
                _vSpeed = (_vSpeed / 2) + .05f;

                if (_vSpeed > 0)
                    _vSpeed = 0;

                _vAccelerate = Time.time + _accelerationDelay;
                velocity.y = _vSpeed * Time.deltaTime;
            }
        }
        

        /*//bring vertical movement to 0 if there is no input or player is no longer controlling ship
        if (_input.vMove == 0)
        {
            
        }*/
    }
}
