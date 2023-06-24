using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorInteractable : MonoBehaviour, IInteractable
{
    [Header("IInteractable variables")]
    [SerializeField] string _interactText;
    [SerializeField] bool _isInteracting = false;

    [Header("Processor variables")]
    [SerializeField] GameObject _processorMenuUI;

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
            ToggleProcessorMenu(isInteracting);
        }

    }
    public void LeaveInteraction()
    {
        if (_isInteracting == true)
        {
            _isInteracting = false;
            ToggleProcessorMenu(false);
        }
    }
    #endregion

    #region Monobehavior
    private void Start()
    {
        _processorMenuUI.SetActive(false);
    }
    #endregion

    #region Custom Methods
    private void ToggleProcessorMenu(bool menuActive)
    {
        Debug.Log("Processor menu: " + menuActive);
        if(_processorMenuUI != null)
        {
            if (menuActive)
            {
                _processorMenuUI.SetActive(true);
            }
            else
            {
                _processorMenuUI.SetActive(false);
            }
        }
    }
    #endregion
}
