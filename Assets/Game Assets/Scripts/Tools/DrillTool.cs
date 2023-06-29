using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrillTool : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Camera _camera;
    //[SerializeField] private InventoryItemSO _itemToReceive;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private int _damagePerSecond;

    private float _timeRemaining = 1f;
    private int _currentHealth;

    // Update is called once per frame
    void Update()
    {
        //if the player clicked
        if(_inputHandler.firePrimary)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            //mousePos.z = 0f;

            Ray ray = _camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            //if the player clicked on a game object
            if (Physics.Raycast(ray, out hit))
            {
                //if the game object is drillable and has health
                if (hit.collider.GetComponent<Drillable>() != null && hit.collider.GetComponent<Health>() != null)
                {
                    Drillable drillable = hit.collider.GetComponent<Drillable>();
                    Health health = hit.collider.GetComponent<Health>();
                    //timer for rate of gain
                    if(_timeRemaining <= 0)
                    {
                        //do damage to drillable game object
                        health.Damage(_damagePerSecond);
                        _currentHealth = health.GetHealth();
                        if (_currentHealth <= 0)
                        {
                            health.Die();
                            drillable.GainOre();
                        }
                        //_inventory.AddItem(_itemToReceive);
                        _timeRemaining = 1f;
                    }
                    else
                    {
                        _timeRemaining -= Time.deltaTime;
                    }
                }
            }
        }
    }
}
