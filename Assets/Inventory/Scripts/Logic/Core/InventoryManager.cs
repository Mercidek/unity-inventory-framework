using System;
using System.IO;
using UnityEngine;

public class InventoryManager
{
    public GameObject Owner { get; private set; }

    private InventorySlot[] slots;
    public int InventorySize => slots.Length;

    private InventorySaveManager saveManager = new InventorySaveManager();

    private readonly string saveFileID;
    private const string defaultSaveFileName = "inventorySave";
    private readonly string savePath;

    ///<summary> Triggers when a slot updates. Passes the modified slot index (int).</summary>
    public event Action<int> OnSlotChanged;
    ///<summary> Triggers when an item is successfully added to the inventory. Passes the item data (ItemSO) and item amount (int).</summary>
    public event Action<ItemSO, int> OnItemAdded;
    ///<summary> Triggers when an item is successfully removed from the inventory. Passes the item data (ItemSO) and item amount (int).</summary>
    public event Action<ItemSO, int> OnItemRemoved;
    ///<summary> Triggers when the inventory fully changes, such as when loading slot data from a saved file.</summary>
    public event Action OnInventoryRefreshed;

    public InventoryManager(string saveFileName)
    {
        saveFileID = string.IsNullOrEmpty(saveFileName) ? defaultSaveFileName : saveFileName;
        savePath   = Path.Combine(Application.persistentDataPath, $"{saveFileID}.json");
    }

    public void initializeSlots(int size)
    {
        slots = new InventorySlot[size];
    }

    public void SaveInventory()
    {
        saveManager.slotsData.Clear();

        for(int i = 0; i < InventorySize; i++)
        {
            ItemSO item = GetItemAtSlot(i, out int amount);

            if(item != null)
            {
                saveManager.slotsData.Add(new SlotSaveData(item.ItemID, amount, i));
            }
            else
            {
                saveManager.slotsData.Add(new SlotSaveData(-1, 0, i));
            }
        }

        string jsonData = JsonUtility.ToJson(saveManager);
        File.WriteAllText(savePath, jsonData);
    }

    public void LoadInventory(ItemDatabaseSO itemDatabase)
    {
        if(!File.Exists(savePath)) return;

        string jsonData = File.ReadAllText(savePath);
        JsonUtility.FromJsonOverwrite(jsonData, saveManager);

        EmptyInventory();

        foreach(var savedSlot in saveManager.slotsData)
        {
            if(savedSlot.slotIndex >= 0 && savedSlot.slotIndex < InventorySize)
            {
                if (savedSlot.itemID != -1)
                {
                    ItemSO itemData = itemDatabase.GetItemFromDatabase(savedSlot.itemID);
                    if (itemData != null)
                    {
                        slots[savedSlot.slotIndex].item = itemData;
                        slots[savedSlot.slotIndex].currentAmount = savedSlot.itemAmount;
                    }
                }
            }
        }
        OnInventoryRefreshed?.Invoke();
    }

    public bool HasSaveFile()
    {
        return File.Exists(savePath);
    }

    // The first loop is for stacking and the second is for adding to an empty slot
    public bool AddItem(ItemSO newItem, int amount)
    {
        if(!CanAddItem(newItem, amount)) return false;

        int leftoverItemCount = amount;

        int count = slots.Length;
        for(int i = 0; i < count; i++)
        {
            ref InventorySlot slot = ref slots[i];
            if(slot.item != null)
            {
                if (newItem.ItemID == slot.item.ItemID && !slot.isFull)
                {
                    int total = slot.currentAmount + leftoverItemCount;
                    if (total > newItem.MaxStackSize)
                    {
                        leftoverItemCount = total - newItem.MaxStackSize;
                        slot.currentAmount = newItem.MaxStackSize;
                        OnSlotChanged?.Invoke(i);
                    }
                    else
                    {
                        slot.currentAmount = total;
                        leftoverItemCount = 0;
                        OnSlotChanged?.Invoke(i);
                    }
                }

                if(leftoverItemCount == 0)
                {
                    OnItemAdded?.Invoke(newItem, amount);
                    return true;
                }
            }
        }

        for(int i = 0; i < count; i++)
        {
            ref InventorySlot slot = ref slots[i];

            if(slot.item == null)
            {
                slot.item = newItem;

                if(leftoverItemCount > newItem.MaxStackSize)
                {
                    slot.currentAmount = newItem.MaxStackSize;
                    leftoverItemCount -= newItem.MaxStackSize;
                    OnSlotChanged?.Invoke(i);
                }
                else
                {
                    slot.currentAmount = leftoverItemCount;
                    leftoverItemCount = 0;
                    OnSlotChanged?.Invoke(i);
                }

                if(leftoverItemCount == 0)
                {
                    OnItemAdded?.Invoke(newItem, amount);
                    return true;
                }
            }
        }
        return true;
    }

    public bool RemoveItem(ItemSO item, int amount)
    {
        if (!CanRemoveItem(item, amount)) return false;

        int remainingRemove = amount;

        int count = slots.Length;
        for (int i = count-1; i >= 0; i--)
        {
            ref InventorySlot slot = ref slots[i];
            if (slot.item != null && item.ItemID == slot.item.ItemID)
            {
                if(slot.currentAmount > remainingRemove)
                {
                    slot.currentAmount -= remainingRemove;
                    remainingRemove = 0;
                    OnSlotChanged?.Invoke(i);
                }
                else
                {
                    remainingRemove -= slot.currentAmount;
                    slot.currentAmount = 0;
                    slot.item = null;
                    OnSlotChanged?.Invoke(i);
                }

                if(remainingRemove == 0)
                {
                    OnItemRemoved?.Invoke(item, amount);
                    return true;
                }
            }
        }
        return true;
    }

    public void RemoveItemAtSlot(int slotIndex, int amount)
    {
        ref InventorySlot slot = ref slots[slotIndex];
        ItemSO itemToRemove = slot.item;
        if (slot.item != null)
        {
            if (slot.currentAmount > amount)
            {
                slot.currentAmount -= amount;
            }
            else
            {
                slot.currentAmount = 0;
                slot.item = null;
                TooltipManager.Instance.HideTooltip();
            }
        }
    }

    // Checks if an item fits in the inventory before adding that item so no item is being wasted
    public bool CanAddItem(ItemSO item, int amount)
    {
        int remaining = amount;
        int count = slots.Length;

        // Check for stacked item slots
        for(int i = 0; i < count; i++)
        {
            ref InventorySlot slot = ref slots[i];
            if(slot.item != null && item.ItemID == slot.item.ItemID && !slot.isFull)
            {
                remaining = Mathf.Max(remaining - (item.MaxStackSize - slot.currentAmount), 0);
                if (remaining == 0) return true;
            }
        }

        // Check for empty slots
        for(int i = 0; i < count; i++)
        {
            ref InventorySlot slot = ref slots[i];
            if(slot.item == null)
            {
                remaining = Mathf.Max(remaining - item.MaxStackSize, 0);
                if (remaining == 0) return true;
            }
        }
        return remaining == 0;
    }

    // Checks if inventory has enough items to remove before performing the actual removal
    public bool CanRemoveItem(ItemSO item, int amount)
    {
        int remaining = amount;
        int count = slots.Length;

        for(int i = 0; i < count; i++)
        {
            ref InventorySlot slot = ref slots[i];
            if (slot.item != null && item.ItemID == slot.item.ItemID)
            {
                remaining -= slot.currentAmount;
                if(remaining <= 0) return true;
            }
        }
        return false;
    }

    ///<summary> Updates a specific slot at the given index in the inventory.</summary>
    public void NotifySlotChange(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            OnSlotChanged?.Invoke(slotIndex);
        }
    }

    ///<summary> Refreshes all slots with null items.</summary>
    public void EmptyInventory()
    {
        for (int i = 0; i < InventorySize; i++)
        {
            slots[i].item = null;
            slots[i].currentAmount = 0;
        }
    }

    public ItemSO GetItemAtSlot(int slotIndex, out int amount)
    {
        amount = slots[slotIndex].currentAmount;
        return slots[slotIndex].item;
    }

    public void SetOwner(GameObject owner)
    {
        Owner = owner;
    }
}

public struct InventorySlot
{
    public ItemSO item;
    public int currentAmount;

    public bool isFull => currentAmount >= item.MaxStackSize;
}
