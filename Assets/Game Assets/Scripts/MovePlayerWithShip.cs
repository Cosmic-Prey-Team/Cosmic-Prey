using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerWithShip : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerState playerState;

    public bool onShip = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            onShip = true;
            playerState.SwitchState(ControlState.FirstPerson);
            Debug.Log("Trigger entered");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onShip = false;
        }
    }

    //private void FixedUpdate()
    //{
    //    if (!onShip)
    //    {
    //        float distance = Vector3.Distance(playerState.transform.position, transform.position);
    //        Debug.Log(distance);
    //        if (distance <= 3)
    //        {
    //            playerState.SwitchState(ControlState.FirstPerson);
    //            onShip = true;
    //        }
    //    }
    //    
    //}

}
