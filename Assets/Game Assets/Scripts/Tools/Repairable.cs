using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repairable : MonoBehaviour
{
    public InventoryItemSO ItemToConsume;
    [SerializeField] private Health _health;
    [SerializeField] private int _amountToRepair;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private float _timeToRepair;

    private float _checkTime;

    private void Start()
    {
        //if the object has a health script, set _health to be it
        //this should be true for everything except the fissures, which'll have the health script on the actual ship
        if (this.gameObject.GetComponent<Health>() != null)
        {
            _health = this.gameObject.GetComponent<Health>();
        }
    }

    public int GetAmountToRepair() { return _amountToRepair; }

    public void repair()
    {
        //if the object needs repairing, the player has waited long enough to repair it, and the player has the item to repair it
        if (_health.GetHealthPercent() != 1f && Time.time > _checkTime && _inventory.RemoveItem(ItemToConsume) != null)
        {
            _health.Heal(_amountToRepair);
            Debug.Log(_health.GetHealth());
            _checkTime = Time.time + _timeToRepair;
        }

        //if you repaired a fissure
        if(this.gameObject.GetComponent<Health>() == null)
        {
            this.gameObject.SetActive(false);
        }
    }
}
