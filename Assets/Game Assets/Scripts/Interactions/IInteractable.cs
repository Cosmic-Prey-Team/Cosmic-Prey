using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(Transform transform);
    string GetInteractText();
    Transform GetTransform();
}
