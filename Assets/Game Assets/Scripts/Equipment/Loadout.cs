using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loadout : MonoBehaviour
{
    public EquipmentSwapping equipment;
    public Image[] ObjectwithImage;       //Gray scale icon
    public Sprite[] spriteToChangeItTo;   //Active icon


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
        //Main idea = selectedEquiment num corrispondeds identically to current weapon array
        switch (selectedEquipment)
        {
            case 0:     //Empty hand selected
                ObjectwithImage[selectedEquipment].sprite = spriteToChangeItTo[selectedEquipment];
                break;
            case 1:     //Melee weapon selected
                ObjectwithImage[selectedEquipment].sprite = spriteToChangeItTo[selectedEquipment]; 
                break;
            case 2:     //Handgun selected
                ObjectwithImage[selectedEquipment].sprite = spriteToChangeItTo[selectedEquipment]; 
                break;
            case 3:     //Drill selected
                ObjectwithImage[selectedEquipment].sprite = spriteToChangeItTo[selectedEquipment]; 
                break;
            case 4:     //Repair tool selected
                ObjectwithImage[selectedEquipment].sprite = spriteToChangeItTo[selectedEquipment];
                break;
        }//End of SWITCH
             
    }
}
