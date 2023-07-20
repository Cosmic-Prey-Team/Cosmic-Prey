using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerState : MonoBehaviour
{
    public ControlState currentState;
    //[SerializeField] List<IInteractable> stateChangingInteractables;
    //[SerializeField] public ShipHelm _shipHelm;

    private void Awake()
    {
        currentState = ControlState.FirstPerson;
        StateAction();
        //_shipHelm = FindObjectOfType<ShipHelm>();
    }

    /*private void OnEnable()
    {
        if (_shipHelm)
            _shipHelm.OnSwitchState += SwitchState;
    }

    private void OnDisable()
    {
        if (_shipHelm)
            _shipHelm.OnSwitchState -= SwitchState;
    }*/

    public void StateAction()
    {
        switch (currentState)
        {
            default:
            case ControlState.FirstPerson:
                OnFirstPersonState();
                break;
            case ControlState.ThirdPerson:
                OnThirdPersonState();
                break;
            case ControlState.Ship:
                OnShipState();
                break;
        }

        //TrySetState(_state);
    }

    public void SwitchState(ControlState newState)
    {
        TrySetState(newState);
    }

    void OnFirstPersonState()
    {
        //change camera view
        Debug.Log("Switched to First Person");
        this.gameObject.GetComponent<StarterAssets.FirstPersonController>().EnterControlPlayer();
    }
    void OnThirdPersonState()
    {
        //Change camera view?
    }
    void OnShipState()
    {
        //change controls
        Debug.Log("Switched to Ship");
        this.gameObject.GetComponent<StarterAssets.FirstPersonController>().EnterControlShip();
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
