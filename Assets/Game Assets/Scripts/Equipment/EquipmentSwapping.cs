using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class EquipmentSwapping : MonoBehaviour
{
    InputHandler _input;

    [SerializeField]
    private int _selectedWeapon = 0;
    public event Action<int> OnSwap;
    
    #region Monobehavior
    private void Awake()
    {
        _input = FindObjectOfType<InputHandler>();
    }
    private void Start()
    {
        SelectWeapon();
    }

    private void Update()
    {
        #region Swap equipment based on Input
        int scrollValue = (int)_input.scroll;

        if (scrollValue != 0)
        {
            if (scrollValue > 0)
            {
                if (_selectedWeapon >= transform.childCount - 1)
                    _selectedWeapon = 0;
                else
                    _selectedWeapon++;
            }
            else if (scrollValue < 0)
            {
                if (_selectedWeapon <= 0)
                    _selectedWeapon = transform.childCount - 1;
                else
                    _selectedWeapon--;
            }

            SelectWeapon();
        }
        #endregion
    }
    #endregion

    public Animator firstpersonAnimator;
    public Animator thirdpersonAnimator;
    
    private void SelectWeapon()
    {
        firstpersonAnimator.SetTrigger("TriggerSwitch");
        thirdpersonAnimator.SetTrigger("TriggerSwitch");
        //enables only selected weapon and disables the rest
        /*
            0 - hand
            1 - drill
            2 - chainsaw
            3 - repair tool
            4 - gun
        */
        int i = 0;
        foreach (Transform equipment in transform)
        {
            if(i == _selectedWeapon)
            {
                //equipment.gameObject.SetActive(true);
                Debug.Log("SelectWeapon(): " + equipment.name);

            }
            else
            {
                equipment.gameObject.SetActive(false);
            }

            i++;
        }
        
        //change selected weapon UI
        OnSwap?.Invoke(_selectedWeapon);
    }
    public int GetSelectedWeapon()
    {
        return _selectedWeapon;
    }

    
}
