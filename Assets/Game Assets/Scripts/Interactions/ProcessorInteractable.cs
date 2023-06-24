using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class ProcessorInteractable : MonoBehaviour, IInteractable
{
    public event Action<bool> OnInteractionChange;

    InputHandler _input;

    [SerializeField] string _interactionText;
    [SerializeField] bool _tapPress = true;
    [SerializeField] string _interactText;
    [SerializeField] GameObject _craftingDisplay;

    private bool _hasInteracted;
    private bool _canvasSetup = false;
    private bool _toggleBool;
    private bool _previousBool;

    private void Awake()
    {
        _input = FindObjectOfType<InputHandler>();
        _craftingDisplay = GameObject.FindGameObjectWithTag("ProcessorCanvas");

    }

    private void Update()
    {
        if (!_canvasSetup)
        {
            _craftingDisplay.SetActive(false);

            _canvasSetup = true;
        }
        #region Using Single Press Interaction
        if (_tapPress)
        {
            if (_toggleBool != _previousBool)
            {
                _previousBool = _toggleBool;
            }
            else
            {
                if (_hasInteracted != false)
                {
                    _hasInteracted = false;
                    _craftingDisplay.SetActive(false);
                    _input.cursorInputForLook = true;
                    OnInteractionChange?.Invoke(false);
                }
            }
        }
        #endregion


    }


    
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
        if (_tapPress)
        {
            if (_hasInteracted == false)
            {
                //start
                Debug.Log("interacting with " + this.transform.name);
                OnInteractionChange?.Invoke(true);
                _input.cursorInputForLook = false;
                _craftingDisplay.SetActive(true);



                //end
                _hasInteracted = true;
            }

            _toggleBool = !_toggleBool;
        }
        else
        {
            _input.cursorInputForLook = false;
            _craftingDisplay.SetActive(true);
            Debug.Log("Interacting with: " + gameObject.name);
        }
        
    }
}
