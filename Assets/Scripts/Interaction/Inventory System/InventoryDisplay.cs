using TMPro;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;  // Reference to the TMP_Text component where the inventory will be displayed

    private void Start()
    {
        // Ensure that inventoryText is assigned before attempting to update it
        if (inventoryText == null)
        {
            Debug.LogError("Inventory Text component is not assigned.");
            return;
        }

        // Update the inventory display when the game starts
        UpdateInventoryDisplay();
    }

    // Call this method whenever you want to refresh the display (e.g., after adding/removing items)
    public void UpdateInventoryDisplay()
    {
        // Clear the current text
        inventoryText.text = "";

        // Get the items from the InventorySystem and format them into a display string
        foreach (var item in InventorySystem.Instance.GetItems())
        {
            // If the item type is Quest or Special, don't show the quantity (amount)
            string itemText = (item.Type == InventorySystem.ItemType.Special || item.Type == InventorySystem.ItemType.Quest)
                ? $"- {item.ItemName}"  // Don't display quantity for Special or Quest items
                : $"- {item.ItemName} x{item.Amount}";  // Display quantity for other items


            // Apply the color based on the item type
            inventoryText.text += ApplyColorBasedOnType(itemText, item.Type) + "\n\n";
        }
    }

    // This method returns the color based on item type
    private string ApplyColorBasedOnType(string itemText, InventorySystem.ItemType itemType)
    {
        string colorHex = "#FFFFFF"; // Default to white if no match

        // Apply color based on item type
        switch (itemType)
        {
            case InventorySystem.ItemType.Special:
                colorHex = "#FFD700";  // Gold color for Special items (use for unique, rare items)
                break;
            case InventorySystem.ItemType.Quest:
                colorHex = "#1E90FF";  // DodgerBlue color for Quest items (use for important story-related items)
                break;
            case InventorySystem.ItemType.Basic:
                colorHex = "#32CD32";  // LimeGreen color for Basic items (use for common or basic items)
                break;
            case InventorySystem.ItemType.Misc:
                colorHex = "#D3D3D3";  // LightGray color for Misc items (use for miscellaneous items)
                break;
        }

        // Wrap the item text with a color tag based on its type
        return $"<color={colorHex}>{itemText}</color>";
    }
}
