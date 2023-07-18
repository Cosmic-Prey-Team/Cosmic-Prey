using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chainsaw : MonoBehaviour
{
    InputHandler _input;

    [Header("Transforms")]
    [Tooltip("Damage source location.")]
    [SerializeField] Transform _hitTransform;

    [Header("Chainsaw Properties")]
    [Tooltip("Damage distance from hit transform.")]
    [SerializeField] float _hitRange = 0.5f;
    [Tooltip("Amount of damage weapon does on hit.")]
    [SerializeField] int _damagePerHit = 1;
    [Tooltip("Seconds between damage.")]
    [SerializeField] float _damageRate = 1f;

    [Header("Events")]
    public UnityEvent OnDamage;
    public UnityEvent OnChainsawStart;
    public UnityEvent OnChainsawEnd;

    private float _timeElapsed;

    //button press events
    bool _isLeftButtonDown = false;
    bool _isLeftPressed = false;

    private void Awake()
    {
        _input = FindObjectOfType<InputHandler>();
    }

    private void Update()
    {
        #region Button Press Events
        //primary fire
        if (_isLeftButtonDown == false)
        {
            if (_input.firePrimary == true)
            {
                OnChainsawStart?.Invoke();

                _isLeftPressed = !_isLeftPressed;

                DoDamage();
                _isLeftButtonDown = true;
            }
        }
        if (_isLeftButtonDown == true)
        {
            if (_input.firePrimary == false)
            {
                OnChainsawEnd?.Invoke();

                _isLeftButtonDown = false;
            }
        }
        #endregion

        //delay timer for damage
        if (_input.firePrimary)
        {
            _timeElapsed -= Time.deltaTime;
            if (_timeElapsed <= 0)
            {
                DoDamage();
            }
        }
    }

    private void DoDamage()
    {

        foreach (var item in GetHitColliders())
        {
            Health health = item.GetComponent<Health>();
            if(health != null)
            {
                Debug.Log("DoDamage()");
                health.Damage(_damagePerHit);
                OnDamage?.Invoke();
            }
        }

        _timeElapsed = _damageRate;
    }

    private Collider[] GetHitColliders()
    {
        //gets all colliders within range
        Collider[] colliders = Physics.OverlapSphere(_hitTransform.position, 0.5f);

        return colliders;
    }
}
