using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int inventorySize = 10;
    public InventorySlot[] slots;

    private void Awake()
    {
        slots = new InventorySlot[inventorySize];
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
                    }
                    else
                    {
                        slot.currentAmount = total;
                        leftoverItemCount = 0;
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
                }
                else
                {
                    slot.currentAmount = leftoverItemCount;
                    leftoverItemCount = 0;
                }
                if (leftoverItemCount == 0) return true;
            }
        }
        return true;
    }

    public bool RemoveItem(ItemSO item, int amount)
    {
        int count = slots.Length;
        for (int i = count; i > 0; i--)
        {
            ref InventorySlot slot = ref slots[i];
            if (slot.currentAmount > amount)
            {
                slot.currentAmount -= amount;
                return true;
            }
            else if(slot.currentAmount <= amount)
            {
                slot.item = null;
                slot.currentAmount = 0;
                return true;
            }
        }
        return false;
    }

    // This method is for checking if an item fits in the inventory before adding that item so no item is being wasted
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
}

public struct InventorySlot
{
    public ItemSO item;
    public int currentAmount;

    public bool isFull => currentAmount >= item.MaxStackSize;
}
