using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;  // Singleton pattern for easy access

    private Dictionary<string, int> inventory = new Dictionary<string, int>();  // Tracks item name and quantity

    private void Awake()
    {
        // Ensures there's only one instance of InventoryManager
        if (Instance == null)
        {
            Instance = this;
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

    // Example method to add items to the inventory
    public void AddItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }
    }
}
