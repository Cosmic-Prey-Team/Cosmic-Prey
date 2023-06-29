using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drillable : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemSO _itemToReceive;
    [SerializeField] private int _amountGained;

    public void GainOre()
    {
        for (int i = 0; i < _amountGained; i++)
        {
            _inventory.AddItem(_itemToReceive);
        }
    }
}
