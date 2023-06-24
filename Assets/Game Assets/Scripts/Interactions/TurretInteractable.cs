using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string _interactText;

    [SerializeField]
    bool _isInteracting = false;
    bool _toggleInteraction;
    bool _savedInteraction;
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
        Debug.Log("Interacting with: " + gameObject.name);
    }

    public void PressInteractionKeyDown(Transform transform)
    {
        //_isInteracting = true;
        if (_toggleInteraction != _savedInteraction)
        {
            _isInteracting = !_isInteracting;
            _toggleInteraction = _savedInteraction;
        }
    }
    public void PressInteractionKeyUp(Transform transform)
    {
        //_isInteracting = false;
    }
}
