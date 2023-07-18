using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoInteractable : MonoBehaviour
{
    [Header("IInteractable variables")]
    [SerializeField] string _interactText;
    [SerializeField] bool _isInteracting = false;

    //[Header("Custom variables")]

    #region IInteractable Methods
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
    #endregion

    #region Monobehavior
    private void Start()
    {

    }
    #endregion

    #region Custom Methods
    private void DoInteractableAction(bool value)
    {
        Debug.Log("DoInteractableAction(" + value+ ")");
    }
    #endregion
}
