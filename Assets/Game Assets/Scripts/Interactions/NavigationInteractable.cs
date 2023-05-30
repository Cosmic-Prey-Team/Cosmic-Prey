using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string _interactText;
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
}
