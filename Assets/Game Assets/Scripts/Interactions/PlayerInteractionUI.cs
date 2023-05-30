using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    PlayerInteract playerInteract;

    TextMeshProUGUI _interactionText;

    private void Awake()
    {
        //getting components
        playerInteract = FindObjectOfType<PlayerInteract>();
        _interactionText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        playerInteract.OnGetInteractable += RefreshUI;
    }
    private void OnDisable()
    {
        playerInteract.OnGetInteractable -= RefreshUI;
    }

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

    //updates UI based on interaction action
    private void RefreshUI(bool hasInteractable)
    {
        if (hasInteractable)
            Show(playerInteract.currentInteractable);
        else
            Hide();
    }
}
