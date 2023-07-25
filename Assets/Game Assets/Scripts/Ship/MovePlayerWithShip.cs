using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerWithShip : MonoBehaviour
{
    //private CharacterController characterController;
    [SerializeField] Transform _playerContainer;
    [SerializeField] ShipController _shipObject;

    private PlayerState playerState;

    public bool onShip = false;

    private void Awake()
    {
        playerState = _playerContainer.GetComponentInChildren<PlayerState>();
        //characterController = playerState.GetComponent<CharacterController>();

        if(_playerContainer != null) _playerContainer.SetParent(null);

    }
    private void OnTriggerStay(Collider other)
    {
        if(other.transform == playerState.transform)
        {
            if(onShip == false) onShip = true;

            if(playerState.currentState == ControlState.SpaceMovement)
                playerState.SwitchState(ControlState.FirstPerson);

            if(_playerContainer != null && _shipObject != null) _playerContainer.SetParent(_shipObject.transform);
            
            //Debug.LogWarning("Trigger entered");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == playerState.transform)
        {
            if(onShip == true) onShip = false;

            if(_playerContainer != null) _playerContainer.SetParent(null);
        }
    }

}
