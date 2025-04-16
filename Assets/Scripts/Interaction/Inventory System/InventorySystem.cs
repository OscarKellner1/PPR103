using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public readonly struct ItemSlot
    {
        private readonly string itemName;
        private readonly int amount;

        public readonly string ItemName => itemName;
        public readonly int Amount => amount;

        public ItemSlot(string name, int amount)
        {
            this.itemName = name;
            this.amount = amount;
        }
    }

    public static InventorySystem Instance;  // Singleton pattern for easy access

    private readonly Dictionary<string, int> inventory = new();  // Tracks item name and quantity

    private void Awake()
    {
        // Ensures there's only one instance of InventoryManager
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

    // Returns the count of a specific item in the player's inventory
    public int GetItemCount(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        return 0;  // Returns 0 if the item isn't in the inventory
    }

    // Returns true if the player is currently holding the specified item
    public bool IsHoldingItem(string itemName)
    {
        // Assuming "holding" an item means it’s the current item the player is using or has equipped
        // This will depend on your specific game logic for holding items
        // For example, you can check if the item is in the player's hand, active slot, etc.
        return inventory.ContainsKey(itemName);  // Replace with actual check if needed
    }

    public void AddItem(string itemName, int amount)
    {
        amount = Mathf.Max(0, amount);
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }
    }

    public void RemoveItem(string itemName, int amount)
    {
        amount = Mathf.Max(0, amount);
        if (!inventory.ContainsKey(itemName)) return;
 
        inventory[itemName] -= amount;
        if (inventory[itemName] <= 0) inventory.Remove(itemName);
    }

    public IEnumerable<ItemSlot> GetItems()
    {
        foreach (KeyValuePair<string, int> item in inventory)
        {
            yield return new ItemSlot(item.Key, item.Value);
        }
    }
}
