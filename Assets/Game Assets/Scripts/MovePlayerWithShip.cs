using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerWithShip : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    //[SerializeField] private GameObject Player;
    //[SerializeField] private Rigidbody rbShip;

    private void Awake()
    {
        //rbShip.GetComponent<Rigidbody>();
    }

    public bool onShip = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //other.gameObject.transform.SetParent(transform);
            onShip = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
           // other.gameObject.transform.SetParent(null);
            onShip = false;
        }
    }

}
