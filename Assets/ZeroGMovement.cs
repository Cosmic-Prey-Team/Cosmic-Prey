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
    [SerializeField]
    private float maxBoostAmount = 2f; //how long to boost for
    [SerializeField]
    private float boostDepecationRate = 0.25f; //how long tank depletes
    [SerializeField]
    private float boostRechargeRate = 0.5f; //how quick cooldown refills
    [SerializeField]
    private float boostMultiplier = 1.4f; //how much faster does this make the player go?
    private bool boosting = false;
    public float currentBoostAmount;



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
    }

    void Setup()
    {
        _mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        currentBoostAmount = maxBoostAmount;
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
    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        upDown1D = context.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
    }

    //public void OnPitchYaw(InputAction.CallbackContext context)
    //{
    //    pitchYaw = context.ReadValue<Vector2>();
    //}

    public void OnBoost(InputAction.CallbackContext context)
    {
        boosting = context.performed;
    }
    #endregion
}
