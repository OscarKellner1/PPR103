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
                // Check PlayerPref condition with comparison options
               
                    int playerPrefValue = PlayerPrefs.GetInt(condition.PPName);
                   
                    switch (condition.PPCond)
                    {
                        
                        case DialogueCondition.PlayerPrefCondition.Is:
                           
                            return playerPrefValue == condition.PPValue;
                        case DialogueCondition.PlayerPrefCondition.IsNot:
                            return playerPrefValue != condition.PPValue;
                        case DialogueCondition.PlayerPrefCondition.GreaterThan:
                            return playerPrefValue > condition.PPValue;
                        case DialogueCondition.PlayerPrefCondition.LessThan:
                            return playerPrefValue < condition.PPValue;
                        default:
                            Debug.Log("Invalid Condition");
                            return false;
                    }
               
                

            case DialogueCondition.ConditionType.HoldingItem:
                // Check if the player is holding the specified item
                HeldObjectHandler heldObjectHandler = GameInfo.GetPlayerCharacter().GetComponent<HeldObjectHandler>();
                if (heldObjectHandler != null && heldObjectHandler.heldObject != null)
                {
                    return heldObjectHandler.heldObject.gameObject.GetComponent<PickupableObject>().ObjectName == condition.itemName;
                }
                return false;

            case DialogueCondition.ConditionType.ItemInInventory:
                // Check if the player has the required amount of the item in their inventory
                int itemCount = InventorySystem.Instance.GetItemCount(condition.itemName);
                switch (condition.itemCondition)
                {
                    case DialogueCondition.ItemCondition.HasExactly:
                        return itemCount == condition.TheNeeded;
                    case DialogueCondition.ItemCondition.HasMoreThan:
                        return itemCount > condition.TheNeeded;
                    case DialogueCondition.ItemCondition.HasLessThan:
                        return itemCount < condition.TheNeeded;
                    default:
                        return false;
                }

            default:
                Debug.LogWarning("Unhandled condition type: " + condition.CType);
                return false;
        }
    }
}
