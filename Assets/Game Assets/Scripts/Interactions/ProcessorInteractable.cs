
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
        Debug.Log("Processor menu: " + menuActive);
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

//old processor class
/*public class ProcessorInteractable : MonoBehaviour, IInteractable
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
}*/