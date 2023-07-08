using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Drillable : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemSO _itemToReceive;
    [SerializeField] private int _amountGained;
    [SerializeField] private float _recoveryTime;
    [Tooltip("If this is checked then ore will be gained when damage is done, otherwise ore is gained when health reaches zero")]
    [SerializeField] bool _gainOverTime = true;
    private bool needsToRecover;
    private float _timeToRecover;
    private Health health;
    private int _maxHealth;
    private int _currentHealth;

    private void Awake()
    {
        health = GetComponent<Health>();
        _maxHealth = health.GetHealth();
        _timeToRecover = _recoveryTime;
    }
    private void Update()
    {
        if (needsToRecover)
        {
            if (_recoveryTime <= 0)
            {
                health.Heal(_maxHealth);
                needsToRecover = false;
                _recoveryTime = _timeToRecover;
            }
            else
            {
                _recoveryTime -= Time.deltaTime;
            }
        }
    }
    private void GainOre()
    {
        for (int i = 0; i < _amountGained; i++)
        {
            _inventory.AddItem(_itemToReceive);
        }
    }
    public void DrillDamage(int _damagePerSecond)
    {
        health.Damage(_damagePerSecond);
        if(_gainOverTime)
        {
            if(HasHealth())
            {
                GainOre();
            }
            else
            {
                needsToRecover = true;
            }
        }
        else
        {
            if(!HasHealth())
            {
                if (needsToRecover == false)
                {
                    GainOre();
                }
                needsToRecover = true;
            }
        }
    }
    private bool HasHealth()
    {
        _currentHealth = health.GetHealth();
        if (_currentHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}