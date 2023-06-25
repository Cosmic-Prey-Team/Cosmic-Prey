using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemInstance
{
    public Inventory Inventory;
    public string ItemName;
    public int ItemCount = 1;
    public string Description;
    public GameObject Sprite;
    public bool Locked = false;

    public InventoryItemInstance(InventoryItemSO item, Inventory inventory)
    {
        Inventory = inventory;
        ItemName = item.ItemName;
        Description = item.Description;
        Sprite = item.Sprite;
    }

    public InventoryItemInstance(InventoryItemInstance item)
    {
        ItemName = item.ItemName;
        Description = item.Description;
        Sprite = item.Sprite;
    }

    public void UpdateSlot(InventoryItemSO item)
    {
        ItemName = item.ItemName;
        Description = item.Description;
        Sprite = item.Sprite;
        //Remove stack left itemcount unchanged, maybe this fixes
        ItemCount = 1;
    }

    public void UpdateSlot(InventoryItemInstance item)
    {
        ItemName = item.ItemName;
        Description = item.Description;
        Sprite = item.Sprite;
        ItemCount = item.ItemCount;
}
}
