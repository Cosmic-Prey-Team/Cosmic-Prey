using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    public ControlState currentState;
    //[SerializeField] List<IInteractable> stateChangingInteractables;
    [SerializeField] public ShipHelm _shipHelm;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody _rb;

    private void Awake()
    {
        currentState = ControlState.FirstPerson;
        StateAction();
        _shipHelm = FindObjectOfType<ShipHelm>();
    }

    private void OnEnable()
    {
        if (_shipHelm)
            _shipHelm.OnSwitchState += SwitchState;
    }

    private void OnDisable()
    {
        if (_shipHelm)
            _shipHelm.OnSwitchState -= SwitchState;
    }

    public void StateAction()
    {
        switch (currentState)
        {
            default:
            case ControlState.FirstPerson:
                OnFirstPersonState();
                break;
            case ControlState.Ship:
                OnShipState();
                break;
            case ControlState.SpaceMovement:
                OnSpaceMovementState();
                break;
        }

        //TrySetState(_state);
    }

    //TODO: Fix bug where state switches right back to FP after pressing helm, find way to switch Invoke Unity Events to Send Messages for other movement controls.

    public void SwitchState(ControlState newState)
    {
        TrySetState(newState);
    }

    void OnFirstPersonState()
    {
        //change camera view
        Debug.Log("Switched to First Person");
        playerInput.SwitchCurrentActionMap("Player");
        this.gameObject.GetComponent<StarterAssets.FirstPersonController>().EnterControlPlayer();
        _rb.isKinematic = true;
        _rb.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
    }
    void OnSpaceMovementState()
    {
        playerInput.SwitchCurrentActionMap("PlayerSpaceControls");
        _rb.isKinematic = false;
        Debug.Log("Switched to FP Space Controls");

    }
    void OnShipState()
    {
        //change controls
        Debug.Log("Switched to Ship Controls");
        this.gameObject.GetComponent<StarterAssets.FirstPersonController>().EnterControlShip();
        _rb.isKinematic = true;
    }
    private bool TrySetState(ControlState newState)
    {

        if (newState == currentState) return false;
        Debug.Log($"State switched to {newState}");
        currentState = newState;

        StateAction();
        return true;//runInEditMode 
    }
}
