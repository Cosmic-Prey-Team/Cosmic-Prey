using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryItemSO : ScriptableObject
{
    public string ItemName;
    [TextArea]
    public string Description;
    public GameObject Sprite;
    //If there were more than two inventories this would be slow since this means always searches every inventory in the game
    public Dictionary<GameObject, InventoryItemInstance> itemsDisplayed;
    public MouseItem mouseItem = new MouseItem();

}

public class MouseItem
{
    public GameObject obj;
    public InventoryItemInstance item;

    //Item in slot being moved to
    public GameObject hoverObj;
    public InventoryItemInstance hoverItem;
}