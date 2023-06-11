using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryItem : ScriptableObject
{
    public string ItemName;
    [TextArea]
    public string Description;
    public Sprite Sprite;
    
}
