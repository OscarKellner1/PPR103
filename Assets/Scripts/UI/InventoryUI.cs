using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text textField;

    private void Update()
    {
        string inventoryList = string.Empty;

        foreach (var slot in InventorySystem.Instance.GetItems())
        {
            inventoryList += slot.ItemName + " (" + slot.Amount + ") \n";
        }

        textField.text = inventoryList;
    }
}
