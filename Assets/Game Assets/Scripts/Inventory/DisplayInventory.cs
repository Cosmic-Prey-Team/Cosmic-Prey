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

public class DisplayInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
    public Inventory inventory;
    public GameObject canvas;
    private int _xStart = -160;
    private int _yStart = 0;
    private int _xSpacing = 160;
    private int _ySpacing = 0;
    Dictionary<GameObject, InventoryItemInstance> itemsDisplayed = new Dictionary<GameObject, InventoryItemInstance>();
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        canvas = GameObject.FindGameObjectWithTag("InventoryCanvas");
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        //change to only update when inventory changes/button to open inventory is pressed
        UpdateSlots();
    }

   public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventoryItemInstance> slot in itemsDisplayed)
        {
            if (slot.Value.ItemName != "Empty")
            {
                slot.Key.GetComponent<Image>().sprite = slot.Value.Sprite.GetComponent<Image>().sprite;
                slot.Key.GetComponent<Image>().color = new Color(1,1,1,1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.ItemCount == 1 ? "" : slot.Value.ItemCount.ToString("n0");
            }
            else{
                slot.Key.GetComponent<Image>().sprite = null;
                slot.Key.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventoryItemInstance>();
        for (int i = 0; i < inventory.inventory.Length; i++)
        {
            Debug.Log(i);
            var obj = Instantiate(inventory.inventory[i].Sprite, Vector3.zero, Quaternion.identity, canvas.transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

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
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;       
        mouseItem.hoverItem = null;
        
    }
    public void OnDragStart(GameObject obj)
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
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDrag(GameObject obj)
    {
        if(mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = new Vector3(Pointer.current.position.x.ReadValue(), Pointer.current.position.y.ReadValue(), 0);
        }
    }
    public void OnDragEnd(GameObject obj)
    {
        if (mouseItem.hoverObj)
        {
            //Since a crafter should be able to use the same inventory script, might be able to just remove from one array and add to the other if it detects a different inventory
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            //Drop Item on ground
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    

    public Vector3 GetPosition(int i)
    {
        //May be displaying incorrectly when resolution is different
        return new Vector3(_xStart + _xSpacing * i, _yStart + -_ySpacing * i,0f);
    }
}

public class MouseItem
{
    public GameObject obj;
    public InventoryItemInstance item;

    //Item in slot being moved to
    public GameObject hoverObj;
    public InventoryItemInstance hoverItem;
}