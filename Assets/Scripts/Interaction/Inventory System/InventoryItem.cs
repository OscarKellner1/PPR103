using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    string itemName;
    [SerializeField]
    int amount;

    public void PickUp()
    {
        InventorySystem.Instance.AddItem(itemName, amount);
        Destroy(gameObject);
    }
}
