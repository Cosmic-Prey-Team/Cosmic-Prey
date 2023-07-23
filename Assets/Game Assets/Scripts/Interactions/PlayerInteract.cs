using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
{
    #region Variables & References
    //events
    public event Action<bool> OnGetInteractable;
    public event Action<bool> OnPressInteractionKey;

    //references
    PlayerState _playerState;
    InputHandler _input;
    ShipInput _shipInput;

    [SerializeField] float _interactionRange = 2f;

    //object player is currently interacting with
    public IInteractable currentInteractable { get; private set; }
    
    //player interaction state
    public bool IsInteracting { get; private set; }
    
    //private variables
    private bool _isButtonDown;
    #endregion

    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _input = GetComponent<InputHandler>();
        _shipInput = GetComponent<ShipInput>();
    }

    private void Update()
    {
        #region Interaction Physics Check
        IInteractable interactable = GetInteractableObject();
        if (interactable != null && currentInteractable != interactable)
        {
            currentInteractable = interactable;
            OnGetInteractable?.Invoke(true);
            //Debug.Log("OnGetInteractable True");

        }
        else if (interactable == null && currentInteractable != null)
        {
            currentInteractable.LeaveInteraction();
            currentInteractable = null;

            OnGetInteractable?.Invoke(false);
            //Debug.Log("OnGetInteractable False");

            //update _isInteracting if player leaves interaction distance
            if (IsInteracting == true) IsInteracting = false;
        }
        #endregion

        #region Button Press Events
        if(_playerState.currentState == ControlState.FirstPerson)
        {
            if (_isButtonDown == false)
            {
                if (_input.interact == true && currentInteractable != null)
                {
                    //toggle _isInteracting on button down
                    IsInteracting = !IsInteracting;

                    OnPressInteractionKey?.Invoke(true);
                    currentInteractable.TriggerInteraction(transform, IsInteracting);
                    _isButtonDown = true;
                    //Debug.Log("PressInteractionKeyDown()");
                }
            }

            if (_isButtonDown == true)
            {
                if (_input.interact == false && currentInteractable != null)
                {
                    OnPressInteractionKey?.Invoke(false);
                    currentInteractable.TriggerInteraction(transform, IsInteracting);
                    _isButtonDown = false;
                    //Debug.Log("PressInteractionKeyUp()");
                }
            }
        }
        else if(_playerState.currentState == ControlState.Ship)
        {
            if (_isButtonDown == false)
            {
                if (_shipInput.interact == true && currentInteractable != null)
                {
                    //toggle _isInteracting on button down
                    IsInteracting = !IsInteracting;

                    OnPressInteractionKey?.Invoke(true);
                    currentInteractable.TriggerInteraction(transform, IsInteracting);
                    _isButtonDown = true;
                    //Debug.Log("PressInteractionKeyDown()");
                }
            }

            if (_isButtonDown == true)
            {
                if (_shipInput.interact == false && currentInteractable != null)
                {
                    OnPressInteractionKey?.Invoke(false);
                    currentInteractable.TriggerInteraction(transform, IsInteracting);
                    _isButtonDown = false;
                    //Debug.Log("PressInteractionKeyUp()");
                }
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