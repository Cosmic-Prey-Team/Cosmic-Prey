using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    private InputHandler _input;

    [Header("Properties")]
    [Tooltip("The speed the harpoon fires at")]
    [SerializeField] float _speed;
    [Tooltip("The duration the harpoon stays out for")]
    [SerializeField] float _lifetime = 5f;
    [Tooltip("The max distance you can be from the tethered object")]
    [SerializeField] float _maxDistance;

    [Header("GameObjects")]
    [Tooltip("The harpoon barrel, so the harpoon knows where to return to")]
    [SerializeField] GameObject _harpoonBarrel;
    [Tooltip("The swivel that makes the turret rotate")]
    [SerializeField] GameObject _swivel;
    [SerializeField] GameObject _whaleHitbox;

    private bool _whaleHit = false;
    private bool _hasBeenFired = false;
    private Rigidbody _rbody;
    private float _timer;

    public bool GetWhaleHit()
    {
        return _whaleHit;
    }

    public float GetMaxDistance()
    {
        return _maxDistance;
    }

    private void Awake()
    {
        _input = FindObjectOfType<InputHandler>();
        _rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.fireSecondary && !_hasBeenFired)
        {
            HarpoonFire();
        }

        //if the harpoon has been fired and either did not hit anything or had the timer ran out
        if (_hasBeenFired && ((Vector3.Distance(transform.position, _harpoonBarrel.transform.position) > _maxDistance && !_whaleHit) || Time.time > _timer))
        {
            Recall();
        }
    }

    void HarpoonFire()
    {
        _hasBeenFired = true;
        _timer = Time.time + _lifetime;

        transform.SetParent(null);

        _rbody.velocity = transform.forward * _speed;
    }

    void Recall()
    {
        _whaleHit = false;
        _hasBeenFired = false;
        _rbody.isKinematic = false;

        transform.SetParent(_harpoonBarrel.transform);

        _rbody.velocity = transform.forward * 0;
        transform.SetPositionAndRotation(_harpoonBarrel.transform.position, _swivel.transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter " + other.name);

        //if the harpoon was fired, and it did not hit the ship
        if (_hasBeenFired && other.gameObject.GetComponent<ShipController>() == null)
        {
            if (other.gameObject == _whaleHitbox)
            {
                _whaleHit = true;
                Debug.Log("Hit Whale");

                _rbody.velocity = transform.forward * 0;

                transform.SetParent(other.transform);
                _rbody.isKinematic = true;
                
                // The code to move the ship to the harpoon is in ShipController.cs
            }
        }
    }
}
