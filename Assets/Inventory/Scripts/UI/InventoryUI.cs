using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotUI slotPrefab;
    private InventoryManager currentInventory;
    private List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    private ObjectPool<InventorySlotUI> slotPool;

    private void Awake()
    {
        slotPool = new ObjectPool<InventorySlotUI>(
            createFunc: () => Instantiate(slotPrefab, slotContainer),
            actionOnGet: slot => slot.gameObject.SetActive(true),
            actionOnRelease: slot => slot.gameObject.SetActive(false),
            actionOnDestroy: slot => Destroy(slot.gameObject),
            collectionCheck: true,
            defaultCapacity: 20,
            maxSize: 100
        );
    }
    public void SetupInventoryUI(InventoryManager inventory)
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (inventorySlots.Count > 0)
        {
            foreach (var oldSlot in inventorySlots)
            {
                oldSlot.OnSlotHovered -= HoverSlot;
                oldSlot.OnSlotClicked -= UseItemAtSlot;
                slotPool.Release(oldSlot);
            }
            inventorySlots.Clear();
        }

        if (currentInventory != null) currentInventory.OnSlotChanged -= UpdateSingleSlot;

        currentInventory = inventory;

        int inventorySize = inventory.InventorySize;
        for(int i = 0; i < inventorySize; i++)
        {
            InventorySlotUI newSlot = slotPool.Get();
            newSlot.transform.SetAsLastSibling();
            inventorySlots.Add(newSlot);
            newSlot.InitSlot(i);

            // Refresh event subscriptions
            newSlot.OnSlotClicked -= UseItemAtSlot;
            newSlot.OnSlotHovered -= HoverSlot;

            newSlot.OnSlotClicked += UseItemAtSlot;
            newSlot.OnSlotHovered += HoverSlot;

            UpdateSingleSlot(i);
        }

        currentInventory.OnSlotChanged += UpdateSingleSlot;
    }

    public void CloseInventoryUI()
    {
        if (inventorySlots.Count > 0)
        {
            foreach (var oldSlot in inventorySlots)
            {
                slotPool.Release(oldSlot);
            }
            inventorySlots.Clear();
        }
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if(currentInventory != null)
        {
            currentInventory.OnSlotChanged -= UpdateSingleSlot;
        }

        if(inventorySlots != null && inventorySlots.Count > 0)
        {
            foreach(var slot in inventorySlots)
            {
                if(slot != null)
                {
                    slot.OnSlotClicked -= UseItemAtSlot;
                    slot.OnSlotHovered -= HoverSlot;
                }
            }
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

    public void UseItemAtSlot(int slotIndex, int useAmount = 1)
    {
        ItemSO item = currentInventory.GetItemAtSlot(slotIndex, out int slotAmount);
        if (item != null && slotAmount >= useAmount)
        {
            currentInventory.RemoveItemAtSlot(slotIndex, useAmount);
        }
    }
}
