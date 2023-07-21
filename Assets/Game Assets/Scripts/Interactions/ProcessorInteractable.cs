
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorInteractable : MonoBehaviour, IInteractable
{
    public event Action<bool> OnInteractionChange;

    [Header("IInteractable variables")]
    [SerializeField] string _interactText;
    [SerializeField] bool _isInteracting = false;

    [Header("Processor variables")]
    [SerializeField] GameObject _craftingDisplay;

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
    private void Awake()
    {

    }
    private void Start()
    {
        _craftingDisplay.SetActive(false);
    }
    #endregion

    #region Custom Methods
    private void ToggleProcessorMenu(bool menuActive)
    {
        Debug.Log("Processor menu active: " + menuActive);
        if(_craftingDisplay != null)
        {
            if (menuActive)
            {
                _craftingDisplay.SetActive(true);
                InputHandler.ModifyCursorState(false, false);
                OnInteractionChange?.Invoke(true);
            }
            else
            {
                _craftingDisplay.SetActive(false);
                InputHandler.ModifyCursorState(true, true);
                OnInteractionChange?.Invoke(true);
            }
        }
    }
    #endregion
}
