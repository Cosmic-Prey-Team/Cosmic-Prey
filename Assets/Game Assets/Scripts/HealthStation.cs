using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStation : MonoBehaviour
{
    [SerializeField] private int _healAmount;
    [SerializeField] private float _healInterval;
    private float _currentHealInterval;

    private void Awake()
    {
        _currentHealInterval = _healInterval;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            Health health = other.GetComponent<Health>();
            if(_currentHealInterval < 0)
            {
                health.Heal(_healAmount);
                _currentHealInterval = _healInterval;
            }
            else
            {
                _currentHealInterval -= Time.deltaTime;
            }
        }
    }
}
