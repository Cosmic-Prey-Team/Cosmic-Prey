using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutOfBounds : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;

    [Tooltip("The damage your ship takes for going out of bounds")]
    [SerializeField] private int _outOfBoundsDamage;
    [Tooltip("The amount of seconds between ticks of damage")]
    [SerializeField] private float _damageRate;

    private bool _outOfBounds;
    private float _timeToDamage;

    private void Awake()
    {
        _outOfBounds = false;
    }

    private void Update()
    {
        if(_outOfBounds)
        {
            if (Time.time > _timeToDamage)
            {
                _timeToDamage = Time.time + _damageRate;
                _playerHealth.Damage(_outOfBoundsDamage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if it was the player
        if (other.gameObject.GetComponent<PlayerState>() != null)
        {
            Debug.Log("in bounds");
            _outOfBounds = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if it was the player
        if (other.gameObject.GetComponent<PlayerState>() != null)
        {
            Debug.Log("out of bounds");
            _outOfBounds = true;
            _timeToDamage = Time.time + _damageRate;
        }
    }
}
