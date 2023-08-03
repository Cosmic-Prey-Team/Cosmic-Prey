using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class Drillable : MonoBehaviour
{
    private Inventory _inventory;
    private Health _health;
    [Tooltip("The item you will receive from drilling")]
    [SerializeField] private InventoryItemSO _itemToReceive;
    [Tooltip("Amount of the item you get")]
    [SerializeField] private int _amountGained;
    [Tooltip("Amount of time for object to recover health and be drillable again")]
    [SerializeField] private float _recoveryTime;
    [Tooltip("If this is checked then the player will gain Amount Gained every damage tick," +
        " if not then it's gained when health is 0")]
    [SerializeField] bool _gainOverTime = true;
    //[SerializeField] Slider _progressBar;
    [SerializeField] Image _progressBar;
    [SerializeField] float _progressBarMoveSpeed;
    private bool needsToRecover;
    private float _timeToRecover;
    private int _maxHealth;
    private int _currentHealth;
    private float _target;
    private bool _gainedLastOre = false;
    private void Awake()
    {
        _health = GetComponent<Health>();
        _maxHealth = _health.GetHealth();
        _currentHealth = _maxHealth;
        _timeToRecover = _recoveryTime;

        if (_progressBar.gameObject.activeInHierarchy == true) _progressBar.gameObject.SetActive(false);
    }
    private void Update()
    {
        UpdateHealthBar(_currentHealth, _maxHealth);
        if (needsToRecover)
        {
            if (_recoveryTime <= 0)
            {
                _health.Heal(_maxHealth);
                _currentHealth = _maxHealth;
                needsToRecover = false;
                _recoveryTime = _timeToRecover;
                _gainedLastOre = false;

                if (_progressBar.gameObject.activeInHierarchy == true) _progressBar.gameObject.SetActive(false);
            }
            else
            {
                _recoveryTime -= Time.deltaTime;

                if (_progressBar.gameObject.activeInHierarchy == false) _progressBar.gameObject.SetActive(true);
            }

        }
    }
    private void GainItem(Transform player)
    {
        _inventory = player.GetComponent<Inventory>();
        _inventory.AddItem(_itemToReceive, _amountGained);
    }
    public void EnableHealthBar(Vector3 uiPos, Transform camTransform)
    {
        if(_progressBar.gameObject.activeInHierarchy == false)
        {
            _progressBar.gameObject.SetActive(true);
        }

        _progressBar.transform.position = uiPos;
        _progressBar.transform.LookAt(transform/*camTransform*/);
        _progressBar.transform.localPosition += _progressBar.transform.forward * -0.05f;
    }
    public void DrillDamage(int _damagePerSecond, Transform player)
    {
        _health.Damage(_damagePerSecond);
        if(_gainOverTime)
        {
            if(HasHealth())
            {
                GainItem(player);
            }
            else
            {
                needsToRecover = true;
                if(!_gainedLastOre)
                {
                    GainItem(player);
                    _gainedLastOre = true;
                }
            }
        }
        else
        {
            if(!HasHealth())
            {
                if (needsToRecover == false)
                {
                    GainItem(player);
                }
                needsToRecover = true;
            }
        }
    }
    private bool HasHealth()
    {
        _currentHealth = _health.GetHealth();
        if (_currentHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        _target = (float)currentHealth/maxHealth;
        //_progressBar.value = Mathf.MoveTowards(_progressBar.value, _target, _progressBarMoveSpeed * Time.deltaTime);
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, _progressBarMoveSpeed * Time.deltaTime);
    }
}