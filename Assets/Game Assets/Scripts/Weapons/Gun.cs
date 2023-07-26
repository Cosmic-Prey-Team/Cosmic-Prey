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
    //[SerializeField] ParticleSystem _muzzleFlash;

    [Header("Gun Settings")]
    [SerializeField] FireMode FireMode = FireMode.Semi;
    [SerializeField] int _damagePerBullet = 1;
    [SerializeField] float _fireRate = 15f;
    [SerializeField] bool _infiniteAmmo = false;
    [SerializeField] int _maxAmmoCount = 10;
    [SerializeField] int _currentAmmoCount;
    [SerializeField] float _maxRange = 200f;

    [Header("Transforms")]
    [SerializeField] Transform _bulletSpawnPoint;
    [SerializeField] Transform _raySpawnPoint;
    [SerializeField] Transform _target;

    //button press events
    bool _isLeftButtonDown = false;
    bool _isLeftPressed = false;

    //private
    private bool _isFiring = false;
    private float _nextTimeToFire = 0f;

    [Header("Events")]
    public UnityEvent OnShoot;
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
            //Debug.LogError("add muzzle flash back in");
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
            //using raycast to determine direction of projectile
            Vector3 rayStartPos = _raySpawnPoint.position;
            Vector3 rayDirection = _raySpawnPoint.forward;
            Vector3 offsetPos = _raySpawnPoint.position + _raySpawnPoint.forward * _maxRange;

            RaycastHit hitInfo;
            if (Physics.Raycast(rayStartPos, rayDirection, out hitInfo, _maxRange))
            {
                _target.position = hitInfo.point;
            }
            else
            {
                _target.position = _raySpawnPoint.TransformPoint(_raySpawnPoint.InverseTransformPoint(offsetPos));
            }

            //rotate spawn point to face target
            _bulletSpawnPoint.LookAt(_target);

            //spawn bullet prefab
            GameObject bulletObject = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            Projectile bullet = bulletObject.GetComponent<Projectile>();
            bullet.Configure(_bulletSpawnPoint, _damagePerBullet);

            OnShoot?.Invoke();
            #endregion
        }
    }
    public void ReloadAmmo()
    {
        Debug.LogError("Reload only for testing");
        _currentAmmoCount = _maxAmmoCount;
        OnAmmoCountChanged?.Invoke(_currentAmmoCount);
    }
    public void SetAmmoCount(int ammoCount)
    {
        _currentAmmoCount = ammoCount;
    }
    public void RefreshAmmoStatus()
    {

    }
}
