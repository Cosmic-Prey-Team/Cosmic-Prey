using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInteractable : MonoBehaviour, IInteractable
{
    [Header("IInteractable variables")]
    [SerializeField] string _interactText;
    [SerializeField] bool _isInteracting = false;
    [SerializeField] Transform _playerTransform;

    [Header("Turrent Variables")]
    [SerializeField] Transform _originalPosition;
    [SerializeField] Transform _usingPosition;
    [SerializeField] Transform _swivelTransform;

    [Header("Look Settings")]
    [SerializeField] float RotationSpeed = 1f;
    [SerializeField] float TopClamp = 60f;
    [SerializeField] float BottomClamp = -60f;

    private float _targetPitch;

    private InputHandler _input;
    private Gun _gun;
    private EquipmentSwapping _equipmentSwapping;
    private StarterAssets.FirstPersonController _firstPersonController;

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
        _gun = GetComponentInChildren<Gun>();
    }
    private void Update()
    {
        
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

            _originalPosition.position = _playerTransform.position;
            _originalPosition.rotation = _playerTransform.rotation;

            _playerTransform.position = _usingPosition.position;
            _playerTransform.rotation = _usingPosition.rotation;

            _equipmentSwapping = FindObjectOfType<EquipmentSwapping>();
            if (_equipmentSwapping != null) _equipmentSwapping.gameObject.SetActive(false);

            _firstPersonController = _playerTransform.GetComponent<StarterAssets.FirstPersonController>();
            _firstPersonController.enabled = false;
        }
        else
        {
            /// on interaction end
            /// return player to original position
            /// enable player equipment
            /// enable player movement and look rotation
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
            transform.Rotate(Vector3.up * rotationVelocity);
        }
    }

    private void EnableShoot(bool enabled)
    {
        _gun.enabled = enabled;
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    #endregion
}
