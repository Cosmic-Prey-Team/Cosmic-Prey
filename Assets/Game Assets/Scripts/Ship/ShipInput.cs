using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipInput : MonoBehaviour
{
    [Header("Ship Input Values")]
    public float thrust;
    public float turn;
    public float vMove;
    public bool interact;
    public Vector2 look;

    private PlayerState _playerState;

    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
    }
    public void OnThrust(InputValue value)
    {
        if(_playerState.currentState == ControlState.Ship)
            thrust = value.Get<float>();
    }

    public void OnTurn(InputValue value)
    {
        if (_playerState.currentState == ControlState.Ship)
            turn = value.Get<float>();
    }

    public void OnUpDown(InputValue value)
    {
        if (_playerState.currentState == ControlState.Ship)
            vMove = value.Get<float>();
    }
    public void OnInteract(InputValue value)
    {
        if (_playerState.currentState == ControlState.Ship)
            interact = value.isPressed;
    }
    public void OnLook(InputValue value)
    {
        if (_playerState.currentState == ControlState.Ship)
            look = value.Get<Vector2>();
    }
}
