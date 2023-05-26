using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
{
    InputHandler _input;

    [SerializeField] float _interactionRange = 2f;

    public IInteractable currentInteractable { get; private set; }

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    private void Update()
    {
        if (_input.interact) //player is interacting
        {
            IInteractable interactable = GetInteractableObject();
            currentInteractable = interactable;
            if (interactable != null)
            {
                interactable.Interact(transform);
            }
        }
    }

    public IInteractable GetInteractableObject()
    {
        
        List<IInteractable> interactableList = new List<IInteractable>();

        //put all interactables in range in a list
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactionRange);
        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out IInteractable interactable))
            {
                interactableList.Add(interactable);
            }
        }

        //find the closest from the list
        IInteractable closestInteractable = null;
        foreach (var interactable in interactableList)
        {
            if(closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {

                if(Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        //return the closest
        return closestInteractable;
    }
}