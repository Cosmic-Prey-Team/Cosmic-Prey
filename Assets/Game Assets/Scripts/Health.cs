using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health;
    
    // events for future use
    public event Action OnHealthChanged;
    public event Action OnDeath;


    
    public void Damage(int damage)
    {
        _health -= damage;
        OnHealthChanged?.Invoke();
    }

    public void Heal(int amount)
    {
        _health += amount;
        OnHealthChanged?.Invoke();
    }

    public void Die()
    {
        Destroy(gameObject);
        OnDeath?.Invoke();
     
    }





}
