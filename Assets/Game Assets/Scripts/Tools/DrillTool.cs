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
    [SerializeField] private int _drillRange;
    [Header("Drill Effects")]
    [Tooltip("Particle effect to play while drilling")]
    [SerializeField] ParticleSystem _drillEffect;
    private float _currentDelayProgress;
    ParticleSystem effect;
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
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if(effect != null) effect.Stop();
            playerAnimator.Play("mc_idle", 2);
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

                if (drillable != null && hit.collider.GetComponent<Health>() != null)
                {
                    //_drillProgressCanvas.gameObject.SetActive(true);
                    drillable.EnableHealthBar(hit.point, _camera.transform);
                    if(Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        effect = Instantiate(_drillEffect);
                        effect.transform.position = hit.point;
                    }
                    //timer for rate of gain
                    if(_currentDelayProgress <= 0)
                    {
                        //do damage to drillable game object
                        drillable.DrillDamage(_damagePerDelay, _player);
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
