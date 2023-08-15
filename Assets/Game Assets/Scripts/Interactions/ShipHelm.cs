using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System;

public class ShipHelm : MonoBehaviour, IInteractable
{
    [Header("IInteractable variables")]
    [SerializeField] string _interactText;
    [SerializeField] bool _isInteracting = false;

    [SerializeField] private PlayerState _playerState = null;

    public event Action<ControlState> OnSwitchState = delegate { };

    private bool _interactionEnabled = true;

    private void Awake()
    {
        if (!_playerState)
        {
            _playerState = FindObjectOfType<PlayerState>();
        }

    }


    public string GetInteractText()
    {
        return _interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void TriggerInteraction(Transform transform, bool isInteracting)
    {
        if (_isInteracting != isInteracting)
        {
            _isInteracting = isInteracting;
            DoInteractableAction(isInteracting);
        }
    }
    public void LeaveInteraction()
    {
        if (_isInteracting == true)
        {
            _isInteracting = false;
            DoInteractableAction(false);
        }
    }
    public bool CanInteract()
    {
        return _interactionEnabled;
    }
    public void DisableInteractions()
    {
        _interactionEnabled = false;
    }
    public void EnableInteractions()
    {
        _interactionEnabled = true;
    }

    public void DoInteractableAction(bool value)
    {
        if (!_playerState._shipHelm)
            _playerState._shipHelm = this;

        if (_playerState.currentState != ControlState.Ship)
        {
            OnSwitchState?.Invoke(ControlState.Ship);
        }
        else if (_playerState.currentState == ControlState.Ship)
        {
            OnSwitchState?.Invoke(ControlState.FirstPerson);
        }
    }
    /*public void Interact(Transform transform)
    {
        if (!_playerState._shipHelm)
            _playerState._shipHelm = this;

        if (_playerState.currentState != ControlState.Ship)
        {
            OnSwitchState?.Invoke(ControlState.Ship);
        }
        else if (_playerState.currentState == ControlState.Ship)
        {
            OnSwitchState?.Invoke(ControlState.FirstPerson);
        }

        //Debug.Log("Interacting with: " + gameObject.name);

    }*/

}