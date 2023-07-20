using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerWithShip : MonoBehaviour
{
    [SerializeField] CharacterController characterController;

    public bool onShip = false;

    //[SerializeField] private GameObject Player;
    //[SerializeField] private Rigidbody rbShip;

    private void Awake()
    {
        //rbShip.GetComponent<Rigidbody>();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        /*if(other.tag == "Player")
        {
            //other.gameObject.transform.SetParent(transform);
            onShip = true;
        }*/
        if (other.gameObject == characterController.gameObject)
        {
            onShip = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (other.tag == "Player")
        {
           // other.gameObject.transform.SetParent(null);
            onShip = false;
        }*/
        if (other.gameObject == characterController.gameObject)
        {
            onShip = false;

        }
    }

}
