using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FireMode { Semi, Auto };
public class Gun : MonoBehaviour
{
    InputHandler _input;

    [Header("Prefabs")]
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] ParticleSystem _muzzleFlash;

    [Header("Gun Settings")]
    [SerializeField] FireMode FireMode = FireMode.Semi;
    [SerializeField] int _damagePerBullet = 1;
    [SerializeField] float _fireRate = 15f;
    [SerializeField] bool _infiniteAmmo = false;
    [SerializeField] int _maxAmmoCount = 10;
    [SerializeField] int _currentAmmoCount;

    [Header("Transforms")]
    [SerializeField] Transform _bulletSpawnPoint;


    //button press events
    bool _isLeftButtonDown = false;
    bool _isLeftPressed = false;

    bool _isRightButtonDown = false;
    bool _isRightPressed = false;

    //private
    private bool _isFiring = false;
    private float _nextTimeToFire = 0f;

    [Space]
    public UnityEvent<int> OnAmmoCountChanged;


    private void Awake()
    {
        _input = FindObjectOfType<InputHandler>();
        _currentAmmoCount = _maxAmmoCount;
    }
    private void Update()
    {
        #region Button Press Events
        //primary firing
        if (_isLeftButtonDown == false)
        {
            if (_input.firePrimary == true)
            {
                _isLeftPressed = !_isLeftPressed;

                if (FireMode == FireMode.Semi) FireAction();
                _isLeftButtonDown = true;
                _isFiring = _isLeftButtonDown;
            }
        }
        if (_isLeftButtonDown == true)
        {
            if (_input.firePrimary == false)
            {
                _isLeftButtonDown = false;
                _isFiring = _isLeftButtonDown;
            }
        }

        //secondary firing
        if (_isRightButtonDown == false)
        {
            if (_input.fireSecondary == true)
            {
                _isRightPressed = !_isRightPressed;

                ReloadAmmo();
                _isRightButtonDown = true;
            }
        }
        if (_isRightButtonDown == true)
        {
            if (_input.fireSecondary == false)
            {
                _isRightButtonDown = false;
            }
        }
        #endregion

        if (FireMode == FireMode.Auto && _isFiring)
        {
            if (Time.time >= _nextTimeToFire) //Time.time >= nextTimeToFire && burst == false
            {
                _nextTimeToFire = Time.time + 1f / _fireRate;
                FireAction();
            }
        }
    }

    public void DesiredAction(bool value)
    {
        Debug.Log("Pressed");
    }
    public void FireAction()
    {
        if(_currentAmmoCount > 0)
        {
            Debug.Log("Fire");

            //create muzzleflash
            Debug.LogError("add muzzle flash back in");
            /*var flash = Instantiate(muzzleflash, bulletSpawn);
            Destroy(flash, 0.2f);*/

            //decrease ammo counter
            if (_infiniteAmmo != true)
            {
                //_currentAmmoCount = _currentAmmoCount - 1;
                _currentAmmoCount--;
                OnAmmoCountChanged?.Invoke(_currentAmmoCount);
                RefreshAmmoStatus();
            }

            #region Rigidbody Method
            //spawn bullet prefab
            GameObject bulletObject = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            Projectile bullet = bulletObject.GetComponent<Projectile>();
            bullet.Configure(transform, _damagePerBullet);
            #endregion
        }
    }
    public void ReloadAmmo()
    {
        Debug.LogError("Reload only for testing");
        _currentAmmoCount = _maxAmmoCount;
        OnAmmoCountChanged?.Invoke(_currentAmmoCount);
    }

    public void RefreshAmmoStatus()
    {

    }
}
