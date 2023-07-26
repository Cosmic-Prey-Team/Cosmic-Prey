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
        for (int equip=0; equip < (spriteToChangeItTo.Length/2); equip++)
        {
            if (equip == selectedEquipment)
                ObjectwithImage[selectedEquipment].sprite = spriteToChangeItTo[selectedEquipment];
            else
                ObjectwithImage[equip].sprite = spriteToChangeItTo[equip+ spriteToChangeItTo.Length / 2];

        }

    }
}
