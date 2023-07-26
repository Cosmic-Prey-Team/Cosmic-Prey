using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events.EquipmentSwapping.cs;

public class LoadoutUI : MonoBehaviour
{
    private EquipmentSwapping equipment;

    // Start is called before the first frame update
    void Awake()
    {
        equipment.OnSwap += refreshEquipment;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void refreshEquipment (int selectedEquipm)
    {

    }
}
