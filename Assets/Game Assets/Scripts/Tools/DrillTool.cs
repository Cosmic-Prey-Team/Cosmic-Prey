using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrillTool : MonoBehaviour
{
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
    [Tooltip("Particle effect to play while drilling")]
    [SerializeField] ParticleSystem _drillEffect;
    [Tooltip("Canvas to display while drilling")]
    [SerializeField] Canvas _drillProgressCanvas;
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
            effect.Stop();
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
                if (hit.collider.GetComponent<Drillable>() != null && hit.collider.GetComponent<Health>() != null)
                {
                    _drillProgressCanvas.gameObject.SetActive(true);
                    if(Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        effect = Instantiate(_drillEffect);
                        effect.transform.position = hit.point;
                    }
                    Drillable drillable = hit.collider.GetComponent<Drillable>();
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
