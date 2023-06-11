using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItemInstance> inventory { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        inventory = new List<InventoryItemInstance>();
    }

    public void AddItem(InventoryItem item)
    {
        foreach (InventoryItemInstance lookFor in inventory) {
            if (lookFor.ItemName == item.ItemName)
            {
                lookFor.ItemCount = lookFor.ItemCount + 1;
                
                return;
            }           
        }

        inventory.Add(new InventoryItemInstance(item));

    }

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
}
