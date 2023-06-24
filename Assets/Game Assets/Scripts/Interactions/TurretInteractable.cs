using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string _interactText;
    [SerializeField] bool _isInteracting = false;

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
        if(_isInteracting != isInteracting) _isInteracting = isInteracting;

        #region Debug (off)
        /*if (_isInteracting != isInteracting)
        {
            _isInteracting = isInteracting;
            Debug.LogWarning("[Turret] _isInteracting = " + _isInteracting);
        }*/
        #endregion
    }
    public void LeaveInteraction()
    {
        if (_isInteracting == true) _isInteracting = false;

        #region Debug (off)
        /*if (_isInteracting == true)
        {
            _isInteracting = false;
            Debug.LogWarning("[Turret] _isInteracting = " + _isInteracting);
        }*/
        #endregion
    }
}
