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
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == playerState.transform)
        {
            onShip = true;
            playerState.SwitchState(ControlState.FirstPerson);
            //Debug.Log("Trigger entered");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == playerState.transform)
        {
            onShip = false;
        }
    }

}
