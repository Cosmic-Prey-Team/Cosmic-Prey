using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using StarterAssets;
using UnityEngine;

public class AnimatorEventFunctions : MonoBehaviour
{
    [SerializeField] private EquipmentSwapping _equipmentSwap;
    public Animator firstpersonAnimator;
    public Animator thirdpersonAnimator;
    public GameObject handObject;
    public GameObject drillObject;
    public GameObject chainsawObject;
    public GameObject repairtoolObject;
    /*
            0 - hand
            1 - drill
            2 - chainsaw
            3 - repair tool
            4 - gun
    */
    void Start()
    {
        
    }

    void Update()
    {
        firstpersonAnimator.SetFloat("moveSpeed", FirstPersonController.targetSpeed);
        thirdpersonAnimator.SetFloat("moveSpeed", FirstPersonController.targetSpeed);
    }

    void swaptoolMesh()
    {
        if(_equipmentSwap.GetSelectedWeapon() == 0)
        {
            handObject.SetActive(true);
            drillObject.SetActive(false);
            chainsawObject.SetActive(false);
            repairtoolObject.SetActive(false);
        }else if(_equipmentSwap.GetSelectedWeapon() == 1)
        {
            handObject.SetActive(false);
            drillObject.SetActive(true);
            chainsawObject.SetActive(false);
            repairtoolObject.SetActive(false);
        }else if(_equipmentSwap.GetSelectedWeapon() == 2)
        {
            handObject.SetActive(false);
            drillObject.SetActive(false);
            chainsawObject.SetActive(true);
            repairtoolObject.SetActive(false);
        }else if(_equipmentSwap.GetSelectedWeapon() == 3)
        {
            handObject.SetActive(false);
            drillObject.SetActive(false);
            chainsawObject.SetActive(false);
            repairtoolObject.SetActive(true);
        }else if(_equipmentSwap.GetSelectedWeapon() == 4)
        {
            handObject.SetActive(false);
            drillObject.SetActive(false);
            chainsawObject.SetActive(false);
            repairtoolObject.SetActive(false);
        }
        
    }
}
