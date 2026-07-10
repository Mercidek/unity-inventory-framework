using System;
using UnityEngine;

public class InventoryManager
{
    public GameObject Owner { get; private set; }

    private InventorySlot[] slots;
    public int InventorySize => slots.Length;

    ///<summary> Triggers when a slot updates. Passes the modified slot index (int).</summary>
    public event Action<int> OnSlotChanged;
    ///<summary> Triggers when an item is successfully added to the inventory. Passes the item data (ItemSO) and item amount (int).</summary>
    public event Action<ItemSO, int> OnItemAdded;
    ///<summary> Triggers when an item is successfully removed from the inventory. Passes the item data (ItemSO) and item amount (int).</summary>
    public event Action<ItemSO, int> OnItemRemoved;

    public InventoryManager()
    {
    }

    public void initializeSlots(int size)
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
