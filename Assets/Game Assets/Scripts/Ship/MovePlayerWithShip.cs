using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerWithShip : MonoBehaviour
{
    //private CharacterController characterController;
    private PlayerState playerState;

    public bool onShip = false;

    private void Awake()
    {
        playerState = FindObjectOfType<PlayerState>();
        //characterController = playerState.GetComponent<CharacterController>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.transform == playerState.transform)
        {
            if(onShip == false) onShip = true;
            if(playerState.currentState == ControlState.SpaceMovement)
                playerState.SwitchState(ControlState.FirstPerson);
            
            Debug.LogWarning("Trigger entered");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == playerState.transform)
        {
            if(onShip == true) onShip = false;
        }
    }

}
