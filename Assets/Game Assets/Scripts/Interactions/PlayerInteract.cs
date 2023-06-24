using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
{
    public event Action<bool> OnGetInteractable;
    public event Action<bool> OnPressInteractionKey;

    InputHandler _input;

    [SerializeField] float _interactionRange = 2f;

    //object player is currently interacting with
    public IInteractable currentInteractable { get; private set; }
    //toggle interaction state
    public bool _isButtonDown { get; private set; }
    public bool _isInteracting; /*{ get; private set; }*/

    private void Awake()
    {
        _input = GetComponent<InputHandler>();

        //OnGetInteractable?.Invoke(false);
    }

    private void Update()
    {
        #region Interaction Physics Check
        IInteractable interactable = GetInteractableObject();
        if (interactable != null && currentInteractable != interactable)
        {
            currentInteractable = interactable;
            OnGetInteractable?.Invoke(true);
            Debug.Log("OnGetInteractable True");

        }
        else if (interactable == null && currentInteractable != null)
        {
            currentInteractable = null;
            OnGetInteractable?.Invoke(false);
            Debug.Log("OnGetInteractable False");
            //update _isInteracting if player leaves interaction distance
            if (_isInteracting == true) _isInteracting = false;
        }
        #endregion

        #region Button Press Events
        if (_isButtonDown == false)
        {
            if (_input.interact == true && currentInteractable != null)
            {
                OnPressInteractionKey?.Invoke(true);
                currentInteractable.PressInteractionKeyDown(transform);
                _isButtonDown = true;
                Debug.Log("PressInteractionKeyDown()");

                //toggle _isInteracting on button down
                _isInteracting = !_isInteracting;
            }
        }

        if (_isButtonDown == true)
        {
            if (_input.interact == false && currentInteractable != null)
            {
                OnPressInteractionKey?.Invoke(false);
                currentInteractable.PressInteractionKeyUp(transform);
                _isButtonDown = false;
                Debug.Log("PressInteractionKeyUp()");
            }
        }
        #endregion
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