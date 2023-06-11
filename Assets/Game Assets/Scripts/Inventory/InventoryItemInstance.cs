using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemInstance
{
    public string ItemName;
    public int ItemCount = 1;
    public string Description;
    public Sprite Sprite;

    public InventoryItemInstance(InventoryItem item)
    {
        ItemName = item.ItemName;
        Description = item.Description;
        Sprite = item.Sprite;
    }
}
