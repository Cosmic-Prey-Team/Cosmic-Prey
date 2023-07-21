using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private InventoryItemSO _item;
    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();
        if(inventory != null)
        {
            other.gameObject.GetComponent<Inventory>().AddItem(_item);
            
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No inventory");
        }

        /*if (other.gameObject.CompareTag("Player"))
        {

        }*/
    }
}
