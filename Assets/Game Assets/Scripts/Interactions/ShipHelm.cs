using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System;

public class ShipHelm : MonoBehaviour, IInteractable
{
    [SerializeField] string _interactText;
    [SerializeField] private PlayerState _playerState = null;

    public event Action<ControlState> OnSwitchState = delegate { };


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

    public void Interact(Transform transform)
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

    }

}
