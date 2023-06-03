using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public event Action<bool> OnGetInteractable;

    InputHandler _input;

    [SerializeField] float _interactionRange = 2f;
    private bool ActionESC = false;

    public event Action<Transform> OnInteract;

    //object player is currently interacting with
    public IInteractable currentInteractable { get; private set; }

    private void Awake()
    {
        _input = GetComponent<InputHandler>();

        OnGetInteractable?.Invoke(false);
    }
    private void Update()
    {
        #region Interaction Physics Check
        IInteractable interactable = GetInteractableObject();
        if (interactable != null && currentInteractable != interactable)
        {
            currentInteractable = interactable;
            OnGetInteractable?.Invoke(true);
        }
        else if (interactable == null && currentInteractable != null)
        {
            currentInteractable = null;
            OnGetInteractable?.Invoke(false);
        }
        #endregion

        //player presses interact button
        if (_input.interact) 
        {
             if (currentInteractable != null)
             {
                currentInteractable.Interact(transform);
                _input.interact = false;
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