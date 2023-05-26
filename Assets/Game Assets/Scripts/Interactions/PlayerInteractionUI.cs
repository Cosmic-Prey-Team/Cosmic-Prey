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
        playerInteract = FindObjectOfType<PlayerInteract>();
        _interactionText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        IInteractable interactable = playerInteract.GetInteractableObject();
        if(interactable != null)
        {
            Show(interactable);
        }
        else
        {
            Hide();
        }
    }
    private void Show(IInteractable interactable)
    {
        _interactionText.gameObject.SetActive(true);
        _interactionText.text = interactable.GetInteractText();
    }
    private void Hide()
    {
        _interactionText.gameObject.SetActive(false);
    }
}
