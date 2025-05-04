using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;


public class AfterActionEvents : MonoBehaviour
{
    [SerializeField] private HeldObjectHandler HOH;

    #region AfterDialogueStuff
    #region MofuEvents
    public void MofuAfterFirstTalk()
    {
        PlayerPrefs.SetInt("MofuTalkedTo", 1);
        InventorySystem.Instance.AddItem("Butt", 2, InventorySystem.ItemType.Basic);
    }
    #endregion

    [System.Serializable]
    public class ActionMethod
    {
        public string methodName; // Identifier like "Mofu"
        public List<InstructionStep> instructions = new List<InstructionStep>();
    }

    public enum StepType
    {
        ChangeVisibility,
        ChangePlayerPrefs,
        ChangeItems,
        ChangeHeldItem,
        ToolTip,       // New Step Type for ToolTip
        CustomEvent    // New Step Type for CustomEvent
    }

    [System.Serializable]
    public class PlayerPrefChange
    {
        public string key;
        public int value;
    }

    [System.Serializable]
    public class ItemChange
    {
        public string itemName;
        public int quantity;
        public InventorySystem.ItemType itemType;  // Add ItemType to ItemChange
    }

    [System.Serializable]
    public class ToolTipData
    {
        public string message;  // The tooltip message
        public Sprite icon;     // The icon/sprite for the tooltip
    }

    [System.Serializable]
    public class InstructionStep
    {
        public string stepName = "New Step";
        public StepType stepType;

        [HideInInspector] public List<GameObject> gameObjectsToAppear = new List<GameObject>();
        [HideInInspector] public List<GameObject> gameObjectsToDisappear = new List<GameObject>();
        [HideInInspector] public List<PlayerPrefChange> playerPrefChanges = new List<PlayerPrefChange>();
        [HideInInspector] public List<ItemChange> itemsToChange = new List<ItemChange>();
        [HideInInspector] public GameObject heldItem;
        [HideInInspector] public bool removingItem;

        // For the new ToolTip and CustomEvent types
        [HideInInspector] public ToolTipData toolTipData;  // Tooltip information (message and icon)
        [HideInInspector] public UnityEvent customEvent;  // Custom Unity event to be invoked
        private Transform holder;

        public void ExecuteStep(HeldObjectHandler HOH)
        {
            switch (stepType)
            {
                case StepType.ChangeVisibility:
                    foreach (GameObject go in gameObjectsToAppear)
                    {
                        if (go != null) go.SetActive(true);
                    }
                    foreach (GameObject go in gameObjectsToDisappear)
                    {
                        if (go != null) go.SetActive(false);
                    }
                    break;

                case StepType.ChangePlayerPrefs:
                    foreach (PlayerPrefChange ppc in playerPrefChanges)
                    {
                        PlayerPrefs.SetInt(ppc.key, ppc.value);
                    }
                    PlayerPrefs.Save();
                    break;

                case StepType.ChangeItems:
                    foreach (ItemChange itemChange in itemsToChange)
                    {
                        if (itemChange.quantity > 0)
                        {
                            InventorySystem.Instance.AddItem(itemChange.itemName, itemChange.quantity, itemChange.itemType);
                        }
                        else if (itemChange.quantity < 0)
                        {
                            InventorySystem.Instance.RemoveItem(itemChange.itemName, Mathf.Abs(itemChange.quantity));
                        }
                        else if (itemChange.quantity == 0)
                        {
                            InventorySystem.Instance.RemoveItem(itemChange.itemName, InventorySystem.Instance.GetItemCount(itemChange.itemName));
                        }
                    }
                    break;

                case StepType.ChangeHeldItem:
                    if (removingItem)
                    {
                        Destroy(HOH.heldObject.gameObject);
                        HOH.heldObject = null;
                        
                    }
                    else
                    {
                        HOH.heldObject = heldItem.GetComponent<PickupableObject>();
                        heldItem.SetActive(true);
                        heldItem.GetComponent<PickupableObject>().isHeld = true;
                        Transform targetHoldPoint = HeldObjectHandler.Instance.GetHoldPointFor(HOH.heldObject.ObjectName);
                        
                        heldItem.GetComponent<PickupableObject>().PickUp(targetHoldPoint);
                        
                    }
                    break;

                case StepType.ToolTip:
                    // Tooltip execution code goes here
                    // You can access the string via toolTipData.message and the sprite via toolTipData.icon
                    Debug.Log("ToolTip: " + toolTipData.message); // Example usage
                    break;

                case StepType.CustomEvent:
                    // Invoke the custom UnityEvent
                    customEvent.Invoke();  // This will trigger the UnityEvent associated with this step
                    break;
            }
        }
    }

    public bool showActionMethods = true;
    public List<ActionMethod> actionMethods = new List<ActionMethod>();

    public void StartInstructions(string methodName)
    {
        ActionMethod foundMethod = actionMethods.Find(m => m.methodName == methodName);
        if (foundMethod != null)
        {
            foreach (InstructionStep step in foundMethod.instructions)
            {
                step.ExecuteStep(HOH);
            }
        }
        else
        {
            Debug.LogWarning($"No method found with name {methodName}!");
        }
    }
    #endregion
}
