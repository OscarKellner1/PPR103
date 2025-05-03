using UnityEngine;

[System.Serializable]
public class DialogueCondition
{
    public enum ConditionType
    {
        PlayerPref,
        HoldingItem,
        ItemInInventory
    }

    public enum PlayerPrefCondition
    {
        Is,
        IsNot,
        GreaterThan,
        LessThan
    }

    public enum ItemCondition
    {
        HasExactly,
        HasMoreThan,
        HasLessThan
    }

    public string CNme;  // Name of the condition (this could be used for debugging purposes)
    public ConditionType CType;  // The condition type (PlayerPref, HoldingItem, ItemInInventory)

    // Player Pref Condition Parameters
    [Header("Player Pref Condition Parameters")]
    public string PPName;  // The Player Pref key for PlayerPref condition
    public PlayerPrefCondition PPCond;  // The comparison to use for PlayerPref condition
    public int PPValue;  // The value to compare to
    

    // Item Condition Parameters (Used for HoldingItem and ItemInInventory conditions)
    [Header("Item Condition Parameters")]
    public string itemName;  // The name of the item
    public ItemCondition itemCondition;  // The comparison for ItemInInventory condition
    public int TheNeeded;  // The amount required for ItemInInventory condition
    
}
