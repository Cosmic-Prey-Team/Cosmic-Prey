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
        equipment = FindObjectOfType<EquipmentSwapping>();
        equipment.OnSwap += refreshEquipment;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void refreshEquipment (int selectedEquipment)
    {
        //Main idea = selectedEquiment num corrispondeds identically to current weapon array
        for (int equip=0; equip < 5 ; equip++)
        {
            // Open hands option
            //Comment me out if there is only 5 icons
            if (selectedEquipment == 0)
            {
                ObjectwithImage[selectedEquipment].sprite = spriteToChangeItTo[selectedEquipment];
                ObjectwithImage[^1].sprite = spriteToChangeItTo[spriteToChangeItTo.Length/2 - 1 ];
                ObjectwithImage[1].sprite = spriteToChangeItTo[7]; //Handsaw debugging
                ObjectwithImage[ObjectwithImage.Length - 2].sprite = spriteToChangeItTo[spriteToChangeItTo.Length - 2]; //Repair gun debugging
            }

            else if (equip == selectedEquipment) 
            {
                ObjectwithImage[selectedEquipment].sprite = spriteToChangeItTo[selectedEquipment];
            }

            //Ensure the icon is back to the inactive icon
            else 
            {
                ObjectwithImage[equip].sprite = spriteToChangeItTo[equip + spriteToChangeItTo.Length / 2];
                ObjectwithImage[5].sprite = spriteToChangeItTo[^1]; //Comment me out if there is only 5 icons

            }

        }

    }
}
