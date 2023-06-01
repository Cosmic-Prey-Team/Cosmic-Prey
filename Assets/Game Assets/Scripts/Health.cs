using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _health;
    
    // events for future use
    public event Action OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        //sets the current health to max
        _health = _maxHealth;
    }

    //returns health value
    public int GetHealth()
    {
        return _health;
    }
    //returns health as a percent of maxHealth
    public float GetHealthPercent()
    {
        return (float)_health / _maxHealth;
    }

    public void Damage(int damage)
    {
        _health -= damage;
        if (_health < 0) _health = 0;
        OnHealthChanged?.Invoke();
    }

    public void Heal(int amount)
    {
        _health += amount;
        if (_health > _maxHealth) _health = _maxHealth;
        OnHealthChanged?.Invoke();
    }

    public void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }





}
