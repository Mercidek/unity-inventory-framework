using System;
using UnityEngine;

public class InventoryManager
{
    private InventorySlot[] slots;
    public int InventorySize => slots.Length;

    public event Action<int> OnSlotChanged;

    public InventoryManager(int size)
    {
        slots = new InventorySlot[size];
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
                if (leftoverItemCount == 0) return true;
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
                if (leftoverItemCount == 0) return true;
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
            if(slot.item != null && item.ItemID == slot.item.ItemID)
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

                if(remainingRemove == 0) return true;
            }
        }
        return true;
    }

    public void RemoveItemAtSlot(int slotIndex, int amount)
    {
        ref InventorySlot slot = ref slots[slotIndex];
        if (slot.item != null)
        {
            if (slot.currentAmount >= amount)
            {
                slot.currentAmount -= amount;
                OnSlotChanged?.Invoke(slotIndex);
            }

            if(slot.currentAmount <= 0)
            {
                slot.item = null;
                TooltipManager.Instance.HideTooltip();
                OnSlotChanged?.Invoke(slotIndex);
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
    public ItemSO GetItemAtSlot(int slotIndex, out int amount)
    {
        amount = slots[slotIndex].currentAmount;
        return slots[slotIndex].item;
    }
}

public struct InventorySlot
{
    public ItemSO item;
    public int currentAmount;

    public bool isFull => currentAmount >= item.MaxStackSize;
}
