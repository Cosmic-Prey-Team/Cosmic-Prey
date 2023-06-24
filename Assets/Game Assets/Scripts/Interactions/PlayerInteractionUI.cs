using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    PlayerInteract playerInteract;

    TextMeshProUGUI _interactionText;

    bool _hasInteractable;
    bool _isInteracting;

    private void Awake()
    {
        //getting components
        playerInteract = FindObjectOfType<PlayerInteract>();
        _interactionText = GetComponentInChildren<TextMeshProUGUI>();

        RefreshUI(false);
    }
    private void OnEnable()
    {
        playerInteract.OnGetInteractable += RefreshUI;
        playerInteract.OnPressInteractionKey += RefreshUI;
    }
    private void OnDisable()
    {
        playerInteract.OnGetInteractable -= RefreshUI;
        playerInteract.OnPressInteractionKey -= RefreshUI;
    }

    #region Show and Hide the UI
    //shows and hides UI
    private void Show(IInteractable interactable)
    {
        _interactionText.gameObject.SetActive(true);
        _interactionText.text = interactable.GetInteractText();
    }
    private void Hide()
    {
        _interactionText.gameObject.SetActive(false);
    }
    #endregion

    //updates UI based on interaction action
    private void RefreshUI(bool actionValue)
    {
        //Debug.Log("RefreshUI()");

        //update interaction values
        _hasInteractable = playerInteract.currentInteractable != null ? true : false;
        _isInteracting = playerInteract.IsInteracting;

        //switch
        if(_hasInteractable == true && _isInteracting == false)
        {
            Show(playerInteract.currentInteractable);
        }
        else if(_hasInteractable == true && _isInteracting == true)
        {
            Hide();
        }
        else if(_hasInteractable == false)
        {
            Hide();
        }
    }
}
