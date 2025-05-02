using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public InventoryDisplay ID;
    // Define an enum for ItemType
    public enum ItemType
    {
        Special,
        Quest,
        Basic,
        Misc,
    }

    // Modify the ItemSlot struct to include ItemType
    public readonly struct ItemSlot
    {
        private readonly string itemName;
        private readonly int amount;
        private readonly ItemType itemType;

        public string ItemName => itemName;
        public int Amount => amount;
        public ItemType Type => itemType;

        public ItemSlot(string name, int amount, ItemType type)
        {
            this.itemName = name;
            this.amount = amount;
            this.itemType = type;
        }
    }

    public static InventorySystem Instance;  // Singleton pattern for easy access

    private readonly Dictionary<string, ItemSlot> inventory = new();  // Tracks item name and associated ItemSlot

    private void Awake()
    {
        // Ensures there's only one instance of InventorySystem
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Adds an item with its type to the inventory
    public void AddItem(string itemName, int amount, InventorySystem.ItemType itemType)
    {
        amount = Mathf.Max(0, amount);
        if (inventory.ContainsKey(itemName))
        {
            var existingItem = inventory[itemName];
            inventory[itemName] = new ItemSlot(itemName, existingItem.Amount + amount, itemType);
        }
        else
        {
            inventory.Add(itemName, new ItemSlot(itemName, amount, itemType));
        }
        ID.UpdateInventoryDisplay();
    }



    // Removes an item with its type from the inventory
    public void RemoveItem(string itemName, int amount)
    {
        amount = Mathf.Max(0, amount);
        if (inventory.ContainsKey(itemName))
        {
            var existingItem = inventory[itemName];
            int newAmount = Mathf.Max(0, existingItem.Amount - amount);
            if (newAmount == 0)
            {
                inventory.Remove(itemName);
            }
            else
            {
                inventory[itemName] = new ItemSlot(itemName, newAmount, existingItem.Type);
            }
        }
        ID.UpdateInventoryDisplay();
    }

    // Returns the count of a specific item in the inventory
    public int GetItemCount(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName].Amount;
        }
        return 0;  // Returns 0 if the item isn't in the inventory
    }

    // Returns true if the player is currently holding the specified item
    public bool IsHoldingItem(string itemName)
    {
        return inventory.ContainsKey(itemName);
    }

    // Returns the entire inventory as an IEnumerable of ItemSlot
    public IEnumerable<ItemSlot> GetItems()
    {
        foreach (var item in inventory.Values)
        {
            yield return item;
        }
    }
}
