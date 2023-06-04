using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string _interactText;

    [SerializeField] GameObject _player;

    private float _moveSpeed;

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
        Debug.Log("Interacting with: " + gameObject.name);
        StarterAssets.FirstPersonController FPS = _player.GetComponent<StarterAssets.FirstPersonController>();
        FPS.StopPlayerMovement();
        Vector3 movementSpeed = new Vector3(0f, 20f, 0f);
        this.GetComponentInParent<Rigidbody>().AddForce(movementSpeed);
    }

   
}
