using System.Collections.Generic;
using UnityEngine;

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
            newSlot.InitSlot(i, this);
            UpdateSingleSlot(i);
        }

        currentInventory.OnSlotChanged += UpdateSingleSlot;
    }

    private void OnDisable()
    {
        if(currentInventory != null)
        {
            currentInventory.OnSlotChanged -= UpdateSingleSlot;
        }
    }

    public void UpdateSingleSlot(int slotIndex)
    {
        ItemSO item = currentInventory.GetItemAtSlot(slotIndex, out int amount);
        inventorySlots[slotIndex].UpdateSlotUI(item, amount);
    }

    public void HoverSlot(int slotIndex)
    {
        ItemSO item = currentInventory.GetItemAtSlot(slotIndex, out int amount);
        if (item != null)
        {
            TooltipManager.Instance.ShowTooltip(item.ItemName, item.ItemDesc);
        }
    }
}
