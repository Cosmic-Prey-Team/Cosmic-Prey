using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class TurretInteractable : MonoBehaviour, IInteractable
{
    [Header("IInteractable variables")]
    [SerializeField] string _interactText;
    [SerializeField] bool _isInteracting = false;

    [Header("Turrent Variables")]
    [SerializeField] Transform _originalPosition;
    [SerializeField] Transform _usingPosition;
    [SerializeField] Transform _lookTarget;
    [SerializeField] Transform _swivelTransform;
    [SerializeField] Transform _swivelParentTransform;

    [Header("Look Settings")]
    [SerializeField] float RotationSpeed = 1f;
    [SerializeField] float TopClamp = 60f;
    [SerializeField] float BottomClamp = -60f;

    [Space]
    public UnityEvent OnInteracted;

    private float _targetPitch;

    private InputHandler _input;
    private Transform _playerTransform;
    private Gun[] _guns;
    private EquipmentSwapping _equipmentSwapping;
    private StarterAssets.FirstPersonController _firstPersonController;
    private CharacterController _characterController;
    private CinemachineVirtualCamera _cinemachine;


    #region IInteractable Methods
    public string GetInteractText()
    {
        return _interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void TriggerInteraction(Transform transform, bool isInteracting)
    {
        if (_isInteracting != isInteracting)
        {
            _isInteracting = isInteracting;
            _playerTransform = transform;
            DoInteractableAction(isInteracting);
        }

    }
    public void LeaveInteraction()
    {
        if (_isInteracting == true)
        {
            _isInteracting = false;
            DoInteractableAction(false);
        }
    }
    #endregion

    #region Monobehavior
    private void Start()
    {
        _input = FindObjectOfType<InputHandler>();
        _guns = GetComponentsInChildren<Gun>();
        EnableGun(false);
        _cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        if (_isInteracting)
        {
            MaintainPosition();
        }
    }
    private void LateUpdate()
    {
        if (_isInteracting)
        {
            TurretRotation();
        }
    }
    #endregion

    #region Custom Methods
    private void DoInteractableAction(bool value)
    {
        Debug.Log("DoInteractableAction(" + value + ")");

        if (value)
        {
            /// on interaction start
            /// save player position
            /// move player to using position
            /// disable player equipment
            /// disable player movement and look rotation
            /// enable gun


            //disable collider
            CapsuleCollider collider = _playerTransform.GetComponentInChildren<CapsuleCollider>();
            if (collider != null) collider.enabled = false;

            //disable first person controller
            _firstPersonController = _playerTransform.GetComponent<StarterAssets.FirstPersonController>();
            _firstPersonController.enabled = false;

            //disable character controller
            _characterController = _playerTransform.GetComponent<CharacterController>();
            _characterController.enabled = false;

            //disable equipment
            _equipmentSwapping = FindObjectOfType<EquipmentSwapping>();
            if (_equipmentSwapping != null) _equipmentSwapping.gameObject.SetActive(false);

            //enable gun
            EnableGun(true);

            //save position
            _originalPosition.position = _playerTransform.position;
            _originalPosition.rotation = _playerTransform.rotation;

            //move player
            //_playerTransform.position = _playerTransform.TransformPoint(_usingPosition.localPosition);
            _playerTransform.position = _usingPosition.position;
            //_playerTransform.localRotation = _usingPosition.localRotation;
            //_playerTransform.LookAt(_lookTarget);
            _cinemachine.m_LookAt = _lookTarget;
        }
        else
        {
            /// on interaction end
            /// return player to original position
            /// enable player equipment
            /// enable player movement and look rotation
            /// disable gun

            //disable collider
            CapsuleCollider collider = _playerTransform.GetComponentInChildren<CapsuleCollider>();
            if (collider != null) collider.enabled = true;

            //enable controller
            if(_firstPersonController != null) _firstPersonController.enabled = true;

            //disable character controller
            if(_characterController != null) _characterController.enabled = true;

            //enable equipment
            if (_equipmentSwapping != null) _equipmentSwapping.gameObject.SetActive(true);

            //disable gun
            EnableGun(false);

            //return player to position
            _playerTransform.position = _originalPosition.position;
            _playerTransform.rotation = _originalPosition.rotation;

            _cinemachine.m_LookAt = null;
        }
    }

    private void TurretRotation()
    {
        // if there is an input
        if (_input.look.sqrMagnitude >= 0.01f)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = 1.0f; /*_firstPersonController.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;*/

            _targetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
            float rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _targetPitch = ClampAngle(_targetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            _swivelTransform.localRotation = Quaternion.Euler(_targetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            _swivelParentTransform.Rotate(Vector3.up * rotationVelocity);
        }
    }
    private void MaintainPosition()
    {
        if(_playerTransform != null)
        {
            _playerTransform.position = _usingPosition.position;
            _playerTransform.rotation = _usingPosition.rotation;
        }
    }

    private void EnableGun(bool enabled)
    {
        //_gun.enabled = enabled;
        foreach (var gun in _guns)
        {
            gun.gameObject.SetActive(enabled);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    #endregion
}
