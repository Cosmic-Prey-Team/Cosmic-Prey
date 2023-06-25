using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void TriggerInteraction(Transform transform, bool value);
    void LeaveInteraction();
    string GetInteractText();
    Transform GetTransform();
}
