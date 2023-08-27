using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Repairable : MonoBehaviour
{
    public UnityEvent OnStartRepairing;
    public UnityEvent OnFullyRepaired;
    KeyTutorial_Fissures keyTutorial_fissures = new KeyTutorial_Fissures();
    private Health _health;
    private Inventory _inventory;

    [Header("Repair Properties")]
    [Tooltip("Is there a health script on this gameobject")]
    [SerializeField] private bool _hasHealthComponent;

    [Tooltip("The item needed to repair the object")]
    [SerializeField] private InventoryItemSO ItemToConsume;

    [Tooltip("How much health the object heals on repair")]
    [SerializeField] private int _healthAmountRegen = 50;

    [SerializeField] 
    Image _progressBar;

    private float _repairProgress = 0;

    //[SerializeField] private int 
    private bool _isRepairing = false;

    private void Awake()
    {
        if(_hasHealthComponent)
            _health = GetComponent<Health>();
        else
            _health = GetComponentInParent<Health>();

        EnableProgressBar(true);
    }

    public bool GetIsRepairing()
    {
        return _isRepairing;
    }
    public int GetAmountToRepair() { return _healthAmountRegen; }
    public void SetAmountToRepair(int amount) { _healthAmountRegen = amount; }

    public void EnableProgressBar(bool active)
    {
        if(_progressBar != null)
        {
            if (active)
            {
                if (_progressBar.gameObject.activeInHierarchy == false) _progressBar.gameObject.SetActive(true);

                _progressBar.fillAmount = _repairProgress;
            }
            else
            {
                if (_progressBar.gameObject.activeInHierarchy == true) _progressBar.gameObject.SetActive(false);
                //fissures left to fix
                keyTutorial_fissures.FissuresFixed();
            }
        }

    }
    public void Repair(Transform player)
    {
        _inventory = player.GetComponent<Inventory>();

        if(_isRepairing == false)
        {
            if(_inventory.RemoveItem(ItemToConsume) != null)
            {
                OnStartRepairing?.Invoke();
                _isRepairing = true;
                Debug.Log("Is repairing");
            }
            else
            {
                Debug.Log("No item to repair");
            }
        }
        if (_isRepairing)
        {
            //increment repair progress
            _repairProgress += 0.2f;
            //Debug.Log("Progress: " + _repairProgress);

            EnableProgressBar(true);

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
