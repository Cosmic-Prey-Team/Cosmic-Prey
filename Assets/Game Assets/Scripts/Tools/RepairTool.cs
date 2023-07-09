using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepairTool : MonoBehaviour
{
    private InputHandler _inputHandler;
    private Camera _camera;
    private Transform _player;

    [Header("Repair Tool Properties")]
    [Tooltip("Interval between repairs.")]
    [SerializeField] private float _timeToRepair;
    [Tooltip("Range of the repair tool.")]
    [SerializeField] private float _repairRange;

    private float _currentTimeToRepair;


    private void Awake()
    {
        _inputHandler = FindObjectOfType<InputHandler>();
        _camera = Camera.main;
        _player = _inputHandler.transform;

        _currentTimeToRepair = _timeToRepair;
    }
    // Update is called once per frame
    void Update()
    {
        //if the player clicked
        if(_inputHandler.firePrimary)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            //mousePos.z = 0f;

            Ray ray = _camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            //if the player clicked on a game object within the range
            if (Physics.Raycast(ray, out hit, _repairRange))
            {
                //if the game object can be repaired
                Repairable repairable = hit.collider.GetComponent<Repairable>();
                if (repairable != null)
                {
                    //timer
                    _currentTimeToRepair -= Time.deltaTime;

                    if(_currentTimeToRepair <= 0)
                    {
                        //Debug.Log("repair()");
                        repairable.Repair(_player);

                        _currentTimeToRepair = _timeToRepair;
                    }
                }
            }
        }
    }
}
