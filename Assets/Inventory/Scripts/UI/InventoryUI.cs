using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotUI slotPrefab;
    private InventoryManager currentInventory;
    private List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();

    public void SetupInventoryUI(InventoryManager inventory)
    {
        currentInventory = inventory;

        int inventorySize = inventory.InventorySize;
        for(int i = 0; i < inventorySize; i++)
        {
            InventorySlotUI newSlot = Instantiate(slotPrefab, slotContainer);
            inventorySlots.Add(newSlot);
        }
    }
}
