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
    [SerializeField] private int _healthAmountRegen;
    //[SerializeField] private float _timeToRepair;

    //[SerializeField] private int 

    private float _checkTime;

    private void Awake()
    {
        if(_hasHealthComponent)
            _health = GetComponent<Health>();
        else
            _health = GetComponentInParent<Health>();
    }

    public int GetAmountToRepair() { return _healthAmountRegen; }

    public void Repair(Transform player)
    {
        _inventory = player.GetComponent<Inventory>();

        if (_hasHealthComponent)
        {


            //if (_health.GetHealthPercent() == 1) OnFullyRepaired?.Invoke();
        }
        else
        {

            //OnFullyRepaired?.Invoke();
        }

        //if the object needs repairing, the player has waited long enough to repair it, and the player has the item to repair it
        if (_health.GetHealthPercent() != 1f && Time.time > _checkTime && _inventory.RemoveItem(ItemToConsume) != null)
        {
            _health.Heal(_healthAmountRegen);
            Debug.Log(_health.GetHealth());
            //_checkTime = Time.time + _timeToRepair;




        }


    }

    
}
