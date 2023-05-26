using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string _interactText;
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform transform)
    {
        Debug.Log(_interactText);
    }
}
