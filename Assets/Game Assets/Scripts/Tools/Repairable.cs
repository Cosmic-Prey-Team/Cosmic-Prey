using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Repairable : MonoBehaviour
{
    public UnityEvent OnFullyRepaired;

    private Health _health;
    private Inventory _inventory;

    [SerializeField] private bool _hasHealthComponent;

    [SerializeField] private InventoryItemSO ItemToConsume;
    [SerializeField] private int _healthAmountRegen = 50;

    //make unserialized after testing
    [SerializeField] private float _repairProgress = 0;

    //[SerializeField] private int 

    private bool _isRepairing = false;

    private void Awake()
    {
        if(_hasHealthComponent)
            _health = GetComponent<Health>();
        else
            _health = GetComponentInParent<Health>();
    }

    public int GetAmountToRepair() { return _healthAmountRegen; }
    public void SetAmountToRepair(int amount) { _healthAmountRegen = amount; }

    public void Repair(Transform player)
    {
        _inventory = player.GetComponent<Inventory>();

        if(_isRepairing == false && _inventory.RemoveItem(ItemToConsume) != null)
        {
            _isRepairing = true;
        }
        if (_isRepairing)
        {
            //increment repair progress
            _repairProgress += 0.2f;
            Debug.Log("Progress: " + _repairProgress);

            if (_repairProgress >= 1)
            {
                _health.Heal(_healthAmountRegen);

                OnFullyRepaired?.Invoke();
                _isRepairing = false;
                _repairProgress = 0;
            }
        }

    }

    
}
