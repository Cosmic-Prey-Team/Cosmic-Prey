using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrillTool : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _damageDelay;
    [SerializeField] private int _damagePerDelay;
    private float _currentDelayProgress;

    private void Awake()
    {
        _currentDelayProgress = _damageDelay;
    }
    // Update is called once per frame
    void Update()
    {
        //if the player clicked
        if(_inputHandler.firePrimary)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();

            Ray ray = _camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            //if the player clicked on a game object
            if (Physics.Raycast(ray, out hit))
            {
                //if the game object is drillable and has health
                if (hit.collider.GetComponent<Drillable>() != null && hit.collider.GetComponent<Health>() != null)
                {
                    Drillable drillable = hit.collider.GetComponent<Drillable>();
                    //timer for rate of gain
                    if(_currentDelayProgress <= 0)
                    {
                        //do damage to drillable game object
                        drillable.DrillDamage(_damagePerDelay);
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
