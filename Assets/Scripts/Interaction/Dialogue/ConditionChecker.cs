using System.Collections.Generic;
using UnityEngine;

public class ConditionChecker : MonoBehaviour
{
    public static ConditionChecker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool AreConditionsMet(List<DialogueCondition> conditions)
    {
        foreach (var condition in conditions)
        {
            if (!CheckCondition(condition))
            {
                return false; // If any condition fails, the whole set fails
            }
        }
        return true; // All conditions met
    }

    private bool CheckCondition(DialogueCondition condition)
    {
        switch (condition.CType)
        {
            case DialogueCondition.ConditionType.PlayerPref:
                // Check if PlayerPref key exists and matches the expected value
                return PlayerPrefs.HasKey(condition.Name) && PlayerPrefs.GetInt(condition.Name, 0) == condition.value;

            case DialogueCondition.ConditionType.HoldingItem:
                // Check if the item is held (assumes InventoryManager handles this logic)
                return InventoryManager.Instance.IsHoldingItem(condition.IName);  // Assuming you have this method

            case DialogueCondition.ConditionType.ItemInInventory:
                // Check if the player has the required amount of the item in their inventory
                return InventoryManager.Instance.GetItemCount(condition.IName) >= condition.Amnt;

            default:
                Debug.LogWarning("Unhandled condition type: " + condition.CType);
                return false;
        }
    }
}
