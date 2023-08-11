using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public InventoryItemInstance[] inventory;
    public int InventorySize;
    [Tooltip("Placeholder item reference")]
    public InventoryItemSO empty;
    [HideInInspector]
    public DisplayCrafting displayCrafting;
    [HideInInspector]
    public DisplayInventory displayInventory;

    // Start is called before the first frame update
    void Awake()
    {
        inventory = new InventoryItemInstance[InventorySize];
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new InventoryItemInstance(empty, this);
        }
        displayInventory = gameObject.GetComponent<DisplayInventory>();
        displayCrafting = gameObject.GetComponent<DisplayCrafting>();
        empty.itemsDisplayed = new Dictionary<GameObject, InventoryItemInstance>();
    }

    public void AddItem(InventoryItemSO item)
    {
        //Increase count of item if already in inventory, else replace an empty slot with it
        foreach (InventoryItemInstance lookFor in inventory)
        {
            if (lookFor.ItemName == item.ItemName)
            {
                //lookFor.ItemCount = lookFor.ItemCount + 1;
                lookFor.ItemCount += 1;
                //Debug.Log("AddItem(): " + lookFor.ItemCount);
                return;
            }
        }

        SetEmptySlot(item);

    }
    public void AddItem(InventoryItemSO item, int amount)
    {
        //Increase count of item if already in inventory, else replace an empty slot with it
        foreach (InventoryItemInstance lookFor in inventory)
        {
            if (lookFor.ItemName == item.ItemName)
            {
                //lookFor.ItemCount = lookFor.ItemCount + 1;
                lookFor.ItemCount += amount;
                return;
            }
        }

        SetEmptySlot(item, amount);

    }
    public void AddStack(InventoryItemInstance item)
    {
        //Increase count of item if already in inventory, else replace an empty slot with it
        Debug.Log(item);
        foreach (InventoryItemInstance lookFor in inventory)
        {
            if (lookFor.ItemName == item.ItemName)
            {
                lookFor.ItemCount += item.ItemCount;

                return;
            }
        }

    }

    public InventoryItemInstance RemoveItem(InventoryItemSO item)
    {
        foreach (InventoryItemInstance lookFor in inventory)
        {
            if (lookFor.ItemName == item.ItemName)
            {
                lookFor.ItemCount = lookFor.ItemCount - 1;
                if (lookFor.ItemCount < 1)
                {
                    lookFor.UpdateSlot(empty);
                }
                return lookFor;
            }
        }
        return null;
    }

    public void RemoveItem(InventoryItemInstance item)
    {
        foreach (InventoryItemInstance lookFor in inventory)
        {
            if (lookFor.ItemName == item.ItemName)
            {
                lookFor.ItemCount = lookFor.ItemCount - 1;
                if (lookFor.ItemCount < 1 && !lookFor.Locked)
                {
                    lookFor.UpdateSlot(empty);
                }
                return;
            }
        }

    }

    public void RemoveStack(InventoryItemInstance item)
    {
        foreach (InventoryItemInstance lookFor in inventory)
        {
            if (lookFor.ItemName == item.ItemName && lookFor.Locked != true)
            {               
                lookFor.UpdateSlot(empty);
                
                return;
            }
        }

    }

    public InventoryItemInstance SetEmptySlot(InventoryItemSO item)
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
    public InventoryItemInstance SetEmptySlot(InventoryItemSO item, int amount)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].ItemName == "Empty")
            {
                inventory[i].UpdateSlot(item);
                AddItem(item, amount - 1);

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

}
