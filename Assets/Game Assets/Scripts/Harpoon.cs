using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    private InputHandler _input;

    [SerializeField] float _speed;
    [SerializeField] float _lifetime = 5f;
    [SerializeField] float _maxDistance;
    [SerializeField] LayerMask _ignoreLayer;
    [SerializeField] GameObject _harpoonBarrel;
    [SerializeField] GameObject _ship;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _swivel;

    private bool _whaleHit = false;
    private bool _hasBeenFired = false;
    private Rigidbody _rbody;
    private float _timer;

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
        if(_hasBeenFired && ((Vector3.Distance(transform.position, _harpoonBarrel.transform.position) > _maxDistance && !_whaleHit) || Time.time > _timer))
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
        _ship.transform.SetParent(null);

        //replace this with having the harpoon return to the turret and velocity being set back to 0
        _rbody.velocity = transform.forward * 0;
        transform.position = _harpoonBarrel.transform.position;
        transform.rotation = _swivel.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.gameObject.GetComponent<ShipController>() == null && other.gameObject.GetComponent<PlayerState>() == null)
        {
            Health otherHealth = other.GetComponent<Health>();
            if (otherHealth != null)
            {
                _whaleHit = true;
                Debug.Log("Hit Damagable");

                _rbody.velocity = transform.forward * 0;

                transform.SetParent(other.transform);
                _rbody.isKinematic = true;
                _ship.transform.SetParent(other.transform);
            }
        }
    }
}
