using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventFunctions : MonoBehaviour
{
    [SerializeField] private EquipmentSwapping _equipmentSwap;
    public GameObject handObject;
    public GameObject drillObject;
    public GameObject chainsawObject;
    /*
            0 - hand
            1 - drill
            2 - chainsaw
            3 - repair tool
            4 - gun
    */
    void swaptoolMesh()
    {
        if(_equipmentSwap.GetSelectedWeapon() == 0)
        {
            handObject.SetActive(true);
            drillObject.SetActive(false);
            chainsawObject.SetActive(false);
        }else if(_equipmentSwap.GetSelectedWeapon() == 1)
        {
            handObject.SetActive(false);
            drillObject.SetActive(true);
            chainsawObject.SetActive(false);
        }else if(_equipmentSwap.GetSelectedWeapon() == 2)
        {
            handObject.SetActive(false);
            drillObject.SetActive(false);
            chainsawObject.SetActive(true);
        }
        
    }
}
