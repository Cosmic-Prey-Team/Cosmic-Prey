using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProcessorCrafting : MonoBehaviour
{
    public float craftInterval = 5f;
    public float craftTimer = 5f;
    public GameObject canvas;
    public Slider progressBar;
    public InventoryItemSO Ammo;
    public InventoryItemSO Panel;
    public InventoryItemSO MachinePart;
    public Vector3 ammoButton = new Vector3(-390, -100, 0f);
    public Vector3 panelButton = new Vector3(-240, -100, 0f);
    public Vector3 partButton = new Vector3(-90, -100, 0f);
    private Queue<InventoryItemSO> itemsToCraft = new Queue<InventoryItemSO>();
    private Inventory inventory;
    public Dictionary<GameObject, InventoryItemSO> craftableItems;
    // Start is called before the first frame update
    void Start()
    {
        craftableItems = new Dictionary<GameObject, InventoryItemSO>();
        inventory = GetComponent<Inventory>();
        //canvas = GameObject.FindGameObjectWithTag("ProcessorCanvas");
        var ammo = Instantiate(Ammo.Sprite, Vector3.zero, Quaternion.identity, canvas.transform);
        Destroy(ammo.transform.GetChild(0).gameObject);
        ammo.GetComponent<RectTransform>().localPosition = ammoButton;
        AddEvent(ammo, EventTriggerType.PointerClick, delegate { OnClick(ammo); });
        craftableItems.Add(ammo, Ammo);

        var panel = Instantiate(Panel.Sprite, Vector3.zero, Quaternion.identity, canvas.transform);
        Destroy(panel.transform.GetChild(0).gameObject);
        panel.GetComponent<RectTransform>().localPosition = panelButton;
        AddEvent(panel, EventTriggerType.PointerClick, delegate { OnClick(panel); });
        craftableItems.Add(panel, Panel);

        var machinePart = Instantiate(MachinePart.Sprite, Vector3.zero, Quaternion.identity, canvas.transform);
        Destroy(machinePart.transform.GetChild(0).gameObject);
        machinePart.GetComponent<RectTransform>().localPosition = partButton;
        AddEvent(machinePart, EventTriggerType.PointerClick, delegate { OnClick(machinePart); });
        craftableItems.Add(machinePart, MachinePart);

    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnClick(GameObject obj)
    {
        if (inventory.inventory[0].ItemCount > itemsToCraft.Count)
        {
            itemsToCraft.Enqueue(craftableItems[obj]);
        }
        else
        {
            //Error message? May also need to let them remove from queue
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (itemsToCraft.Count > 0 && inventory.inventory[0].ItemCount > 0)
        {
            progressBar.value = 1f - craftTimer / craftInterval;
            if (craftTimer <= 0)
            {
                craftTimer = craftInterval;
                CraftItem(itemsToCraft.Dequeue());
            }
            craftTimer -= Time.deltaTime;
        }
    }

    public void CraftItem(InventoryItemSO item)
    {
        inventory.RemoveItem(inventory.inventory[0]);
        inventory.AddItem(item);
        progressBar.value = 0;
    }
}
