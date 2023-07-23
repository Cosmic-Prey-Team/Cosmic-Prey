using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Progress;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DisplayCrafting : MonoBehaviour
{
    InputHandler _input;
    [HideInInspector]
    public Inventory inventory;
    public GameObject canvas;
    public InventoryItemSO Ore;
    private RectTransform[] slotPlaceholders;
    Dictionary<GameObject, InventoryItemInstance> itemsDisplayed;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        itemsDisplayed = inventory.empty.itemsDisplayed;
        //canvas = GameObject.FindGameObjectWithTag("ProcessorCanvas");
        slotPlaceholders = canvas.GetComponentsInChildren<RectTransform>(true);
        CreateSlots();
        inventory.AddItem(Ore);
        inventory.inventory[0].ItemCount--;
        inventory.inventory[0].Locked = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (_input.open)
        {
            //Open
        }

        if (_input.close)
        {
            //Close
        }*/
    }

    public void CreateSlots()
    {
        for (int i = 0; i < inventory.inventory.Length; i++)
        {
            //Debug.Log(i);
            var obj = Instantiate(inventory.inventory[i].Sprite, Vector3.zero, Quaternion.identity, canvas.transform);
            Debug.Log(slotPlaceholders[i]);
            obj.GetComponent<RectTransform>().localPosition = slotPlaceholders[i + 1].localPosition; //GetComponentsInChildren includes parent in array so starts at index 1
            //slotPlaceholders[i + 1].gameObject.SetActive(false);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });


            itemsDisplayed.Add(obj, inventory.inventory[i]);
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        inventory.empty.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            inventory.empty.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
    public void OnExit(GameObject obj)
    {
        inventory.empty.mouseItem.hoverObj = null;
        inventory.empty.mouseItem.hoverItem = null;

    }
    public void OnDragStart(GameObject obj)
    {
        if (itemsDisplayed[obj].Locked != true)
        {
            //Display sprite of item to show being dragged
            var mouseObject = new GameObject();
            mouseObject.transform.SetParent(canvas.transform);
            var rt = mouseObject.gameObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(75, 75);
            if (itemsDisplayed[obj].ItemName != "Empty")
            {
                var img = mouseObject.AddComponent<Image>();
                img.sprite = itemsDisplayed[obj].Sprite.GetComponent<Image>().sprite;
                img.raycastTarget = false;
            }
            inventory.empty.mouseItem.obj = mouseObject;
            inventory.empty.mouseItem.item = itemsDisplayed[obj];
        }
    }
    public void OnDrag(GameObject obj)
    {
        if (inventory.empty.mouseItem.obj != null)
        {
            inventory.empty.mouseItem.obj.GetComponent<RectTransform>().position = new Vector3(Pointer.current.position.x.ReadValue(), Pointer.current.position.y.ReadValue(), 0);
        }
    }
    public void OnDragEnd(GameObject obj)
    {
        if (inventory.empty.mouseItem.item != null && inventory.empty.mouseItem.hoverObj)
        {
            if (inventory.empty.mouseItem.hoverItem.ItemName == inventory.empty.mouseItem.item.ItemName)
            {
                if (inventory.empty.mouseItem.hoverItem.Inventory != inventory.empty.mouseItem.item.Inventory)
                {
                    inventory.empty.mouseItem.hoverItem.Inventory.AddStack(inventory.empty.mouseItem.item);
                    inventory.empty.mouseItem.item.Inventory.RemoveStack(inventory.empty.mouseItem.hoverItem);
                }
            }
            else if(inventory.empty.mouseItem.hoverItem.Locked != true)
            {
                inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[inventory.empty.mouseItem.hoverObj]);
            }           
        }

        Destroy(inventory.empty.mouseItem.obj);
        inventory.empty.mouseItem.item = null;
    }




private void Awake()
    {
        _input = FindObjectOfType<InputHandler>();
    }
    

    
}
