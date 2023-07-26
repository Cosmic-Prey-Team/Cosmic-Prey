using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Loadout : MonoBehaviour
{
    private EquipmentSwapping equipment;
    public Sprite[] selectedLoadout_Array;
    public SpriteRenderer inactiveLoadout;
   

    // Start is called before the first frame update
    void Awake()
    {
        equipment.OnSwap += refreshEquipment;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void refreshEquipment (int selectedEquipment)
    {
        switch (selectedEquipment)
        {
            case 0:     //Empty hand selected
                inactiveLoadout.sprite = selectedLoadout_Array[selectedEquipment];               
                break;
            case 1:     //Melee weapon selected
                inactiveLoadout.sprite = selectedLoadout_Array[selectedEquipment];
                break;
            case 2:     //Handgun selected
                inactiveLoadout.sprite = selectedLoadout_Array[selectedEquipment];
                break;
            case 3:     //Drill selected
                inactiveLoadout.sprite = selectedLoadout_Array[selectedEquipment];
                break;
            case 4:     //Repair tool selected
                inactiveLoadout.sprite = selectedLoadout_Array[selectedEquipment];
                break;
        }//End of SWITCH
             
    }
}
