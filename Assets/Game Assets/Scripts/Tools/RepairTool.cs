using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepairTool : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Camera _camera;
    [SerializeField] private InventoryItemSO _machinePart, _panel;
    [SerializeField] private float _timeToRepair, _repairRange;

    

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
            if (Physics.Raycast(ray, out hit) && Vector3.Distance(this.gameObject.transform.position, hit.collider.transform.position) <= _repairRange)
            {
                //if the game object can be repaired
                if(hit.collider.GetComponent<Repairable>() != null)
                {
                    Repairable repairable = hit.collider.GetComponent<Repairable>();
                    repairable.repair();
                }
            }
        }
    }
}
