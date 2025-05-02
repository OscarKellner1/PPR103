using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    private string itemName;  // Name of the item (e.g., "Red Berry")

    [SerializeField]
    private int amount = 1;  // Default amount is 1 if not set

    [SerializeField]
    private InventorySystem.ItemType itemType;  // Now referencing InventorySystem's ItemType

    public void PickUp()
    {
        // Add the item to the inventory system with the itemName, amount, and itemType
        InventorySystem.Instance.AddItem(itemName, amount, itemType);

        // Optionally destroy the object after pickup (remove it from the world)
        Destroy(gameObject);
    }

    public InventorySystem.ItemType GetItemType()
    {
        return itemType;
    }
}
