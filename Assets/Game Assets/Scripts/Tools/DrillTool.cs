using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrillTool : MonoBehaviour
{
    public Animator playerAnimator;
    private InputHandler _inputHandler;
    private Transform _player;
    private Camera _camera;
    [Header("Drill Properties")]
    [Tooltip("The time between each damage tick")]
    [SerializeField] private float _damageDelay;
    [Tooltip("The amount of damage done every tick")]
    [SerializeField] private int _damagePerDelay;
    [Tooltip("The distance you can drill from")]
    [SerializeField] private float _drillRange;
    [Header("Drill Effects")]
    [Tooltip("Effect to play while drilling")]
    [SerializeField] GameObject _drillEffect;
    private float _currentDelayProgress;
    private GameObject effect;
    private void Awake()
    {
        _currentDelayProgress = _damageDelay;
        _inputHandler = FindObjectOfType<InputHandler>();
        _camera = Camera.main;
        _player = _inputHandler.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(effect != null)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame || Vector3.Distance(this.transform.position, effect.transform.position) > _drillRange)
            {
                Destroy(effect);
            }
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            playerAnimator.Play("mc_mine", 2);
        }
        //if the player clicked
        if (_inputHandler.firePrimary)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();

            Ray ray = _camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            //if the player clicked on a game object
            if (Physics.Raycast(ray, out hit, _drillRange))
            {
                //if the game object is drillable and has health
                Drillable drillable = hit.collider.GetComponent<Drillable>();
                Breakable breakable = hit.collider.GetComponent<Breakable>();


                if (drillable != null && hit.collider.GetComponent<Health>() != null)
                {
                    if(effect != null)
                    {
                        effect.transform.position = hit.point;
                    }
                    //_drillProgressCanvas.gameObject.SetActive(true);
                    drillable.EnableHealthBar(hit.point, _camera.transform);
                    if(Mouse.current.leftButton.isPressed)
                    {
                        if(effect == null)
                        {
                            effect = Instantiate(_drillEffect, hit.point, Quaternion.identity);
                        }
                    }
                    //timer for rate of gain
                    if(_currentDelayProgress <= 0)
                    {
                        //do damage to drillable game object
                        drillable.DrillDamage(_damagePerDelay, _player);

                        if (breakable != null)
                        {
                            //Debug.Log("hittpoint: " + hit.point);
                            breakable.explosionPoint = hit.point;
                        }
                        _currentDelayProgress = _damageDelay;
                    }
                    else
                    {
                        _currentDelayProgress -= Time.deltaTime;
                    }
                }
            }
        }
    }
}
