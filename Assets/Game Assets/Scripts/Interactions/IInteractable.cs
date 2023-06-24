using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void PressInteractionKeyDown(Transform transform);
    void PressInteractionKeyUp(Transform transform);

    string GetInteractText();
    Transform GetTransform();
}
