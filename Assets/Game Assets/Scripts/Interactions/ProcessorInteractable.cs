using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string _interactText;

    bool _isInteracting = false;
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
        _isInteracting = true;
    }
    public void PressInteractionKeyUp(Transform transform)
    {
        _isInteracting = false;
    }
}
