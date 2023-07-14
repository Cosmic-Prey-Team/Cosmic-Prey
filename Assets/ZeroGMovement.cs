using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ZeroGMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("==== Player Space Movement Settings ===")]
    [SerializeField]
    private float rollTorque = 500f;
    [SerializeField]
    private float thrust = 100f;
    [SerializeField]
    private float upThrust = 50f;
    [SerializeField]
    private float strafeThrust = 50f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float leftRightGlideReduction = 0.111f;

    private Camera _mainCam;


    [Header("==== Boost Settings ===")]
    [Tooltip("The amount of time a boost will last before it depreciates and starts to reset.")]
    [SerializeField]
    private float maxBoostAmount = 2f; //how long to boost for
    [SerializeField]
    [Tooltip("Rate at which boost depreciates.")]
    private float boostDepecationRate = 0.25f; //how long tank depletes
    [Tooltip("Rate at which the boost recharges.")]
    [SerializeField]
    private float boostRechargeRate = 0.5f; //how quick cooldown refills
    [Tooltip("How much faster the boost makes the player go.")]
    [SerializeField]
    private float boostMultiplier = 1.4f; //how much faster does this make the player go?
    private bool boosting = false;
    public float currentBoostAmount;

    [Header("====Mouse Movement Settings ====")]
    private PlayerInput _playerInput;
    private InputHandler _input;
    private const float _threshold = 0.01f;
    private float _rotationVelocity;
    // cinemachine
    private float _cinemachineTargetPitch;
  
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    [Tooltip("Rotation speed of the character")]
    public float RotationSpeed = 1.0f;



    Rigidbody rb;

    //input Values

    private float thrust1D;
    private float upDown1D;
    private float strafe1D;
    private float roll1D;
    private Vector2 pitchYaw;
    private float glide, verticalGlide, horizontalGlide = 0.0f;

    void Start()
    {
        Setup();
    }

    void FixedUpdate()
    {
        HandleBoosting();
        HandleMovement();
        CameraRotation();
    }

    void Setup()
    {
        _mainCam = Camera.main;
        _input = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        currentBoostAmount = maxBoostAmount;

#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
    }

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }
    void HandleBoosting()
    {
        if (boosting && currentBoostAmount > 0f)
        {
            currentBoostAmount -= boostDepecationRate;
            if (currentBoostAmount <= 0f)
            {
                boosting = false;
            }
        }
        else
        {
            if (currentBoostAmount < maxBoostAmount)
            {
                currentBoostAmount += boostRechargeRate;
            }
        }
    }

    void HandleMovement()
    {
        //Roll
        rb.AddTorque(-_mainCam.transform.forward * roll1D * rollTorque * Time.deltaTime);
        //Pitch
        //rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime);
        //// Yaw
        //rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);

        // thrust
        if (thrust1D > 0.1f || thrust1D < -0.1f)
        {
            //Debug.Log("Thrust " + thrust1D);
            float currentThrust;

            if (boosting)
            {
                currentThrust = thrust * boostMultiplier;
            }
            else
            {
                currentThrust = thrust;
            }


            rb.AddForce(_mainCam.transform.forward * thrust1D * currentThrust * Time.deltaTime);
            glide = thrust;
        }
        else
        {
            rb.AddForce(_mainCam.transform.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction;
        }

        //Up/Down

        if (upDown1D > 0.1f || upDown1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.up * upDown1D * upThrust * Time.deltaTime);
            verticalGlide = upDown1D * upThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.up * verticalGlide * Time.deltaTime);
            verticalGlide *= upDownGlideReduction;
        }

        // STRAFING
        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddForce(_mainCam.transform.right * strafe1D * strafeThrust * Time.deltaTime);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else
        {
            rb.AddForce(_mainCam.transform.right * horizontalGlide * Time.deltaTime);
            verticalGlide *= leftRightGlideReduction;
        }

    }

    #region Input Methods
    public void OnThrust(InputValue value)
    {
        thrust1D = value.Get<float>();
        
    }

    public void OnStrafe(InputValue value)
    {
        strafe1D = value.Get<float>();
        //Debug.Log("Strafe" + strafe1D);

    }

    public void OnUpDown(InputValue value)
    {
        upDown1D = value.Get<float>();
        //Debug.Log("UpDown");

    }

    public void OnRoll(InputValue value)
    {
        roll1D = value.Get<float>();
        //Debug.Log("Roll");

    }

    //public void OnPitchYaw(InputAction.CallbackContext context)
    //{
    //    pitchYaw = context.ReadValue<Vector2>();
    //}

    public void OnBoost(InputValue value)
    {
        if (value.Get<float>() != 0)
            boosting = true;
        else
            boosting = false;
    }

    private void CameraRotation()
    {
        // if there is an input
        if (_input.look.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            transform.Rotate(Vector3.up * _rotationVelocity);

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
