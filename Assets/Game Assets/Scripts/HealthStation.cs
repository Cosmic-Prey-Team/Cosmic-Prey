using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class HealthStation : MonoBehaviour
{
    [SerializeField] private int _healAmount;
    [SerializeField] private float _healInterval;
    [SerializeField] ParticleSystem _healingEffect;
    private float _currentHealInterval;

    private void Awake()
    {
        _currentHealInterval = _healInterval;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            if(_healingEffect != null)
            {
                if (!_healingEffect.isPlaying)
                {
                    _healingEffect.Play();
                }
            }
            else
            {
                Debug.LogWarning("Missing healing effect.");
            }
            
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
    private void OnTriggerExit(Collider other)
    {
        if (_healingEffect == null) return;

        if(other.GetComponent<Health>())
        {
            _healingEffect.Stop();
        }
    }
}
