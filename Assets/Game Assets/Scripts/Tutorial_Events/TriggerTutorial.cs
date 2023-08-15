using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : Tutorial_Steps
{
    private bool isCurrentTutorial = false;
    public Transform HitTransform;
    GameObject Inventory;
    public override void CheckIf_Happending()
    {
        isCurrentTutorial = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isCurrentTutorial)
            return;

        //The player reaches a collision space
        if (other.transform == HitTransform)
        {
            GameTutorial_Manager.Instace.TutorialIncrement();
            isCurrentTutorial = false;
        }
        //InventoryisEmpty();
    }
/*
    //Checking if inventory is empty
    public void InventoryisEmpty()
    {
        Inventory = GameObject.Find("Inventory");

        if (Inventory.transform.childCount > 4)
        {
            GameTutorial_Manager.Instace.TutorialIncrement();
            isCurrentTutorial = false;
        }        
        else
            isCurrentTutorial = true;

    }*/
}