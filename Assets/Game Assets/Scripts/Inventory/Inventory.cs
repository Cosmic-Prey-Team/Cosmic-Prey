using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    
    public InventoryItemInstance[] inventory;
    public InventoryItem empty;

    // Start is called before the first frame update
    void Awake()
    {
        inventory = new InventoryItemInstance[3];
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new InventoryItemInstance(empty);
        }

        }
    
    public void AddItem(InventoryItem item)
    {
        //Increase count of item if already in inventory, else replace an empty slot with it
        Debug.Log(item);
        foreach (InventoryItemInstance lookFor in inventory) {
            if (lookFor.ItemName == item.ItemName)
            {
                lookFor.ItemCount = lookFor.ItemCount + 1;
                
                return;
            }           
        }

        SetEmptySlot(item);

    }

    public InventoryItemInstance SetEmptySlot(InventoryItem item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].ItemName == "Empty")
            {
                inventory[i].UpdateSlot(item);
                return inventory[i];
            }
        }
        return null;
    }

    public void MoveItem(InventoryItemInstance item1, InventoryItemInstance item2)
    {
        InventoryItemInstance temp = new InventoryItemInstance(item2);
        item2.UpdateSlot(item1);
        item1.UpdateSlot(temp);
    }

    //Old code for removing items, now probably handled by drag events
    /**
    public void RemoveItem(InventoryItem item)
    {
        foreach (InventoryItemInstance lookFor in inventory)
        {
            if (lookFor.ItemName == item.ItemName)
            {
                if (lookFor.ItemCount <= 1)
                {
                    inventory.Remove(lookFor);
                }
                else
                {
                    lookFor.ItemCount = lookFor.ItemCount--;
                }
                return;
            }
        }
        
    }

    public int RemoveStack(InventoryItem item)
    {
        foreach (InventoryItemInstance lookFor in inventory)
        {
            if (lookFor.ItemName == item.ItemName)
            {
                int numRemoved = lookFor.ItemCount;
                inventory.Remove(lookFor);
                return numRemoved;
            }
        }
        

        return -1;
    }
    **/
}
