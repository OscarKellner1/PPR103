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
    public string CNme;
    public ConditionType CType;

    // Add a header to group the following parameters under Player Pref
    [Header("Player Pref Condition Parameters")]
    public string Name;   // Only relevant for PlayerPref condition
    public int value;    // Only relevant for PlayerPref condition

    // Add a header to group the following parameters under Item conditions
    [Header("Item Condition Parameters")]
    public string IName;  // Used for HoldingItem and ItemInInventory conditions
    public int Amnt;  // Only relevant for ItemInInventory condition (amount of items required)
}
