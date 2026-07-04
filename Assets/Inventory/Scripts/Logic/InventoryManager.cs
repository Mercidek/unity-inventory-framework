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
        int count = slots.Length;
        for(int i = 0; i < count; i++)
        {
            ref InventorySlot slot = ref slots[i];
            if(newItem.ItemID == slot.item.ItemID && !slot.isFull)
            {
                slot.currentAmount += amount;
                return true;
            }
        }

        for(int i = 0; i < count; i++)
        {
            ref InventorySlot slot = ref slots[i];
            if (slot.currentAmount == 0)
            {
                slot.item = newItem;
                slot.currentAmount = amount;
                return true;
            }
        }
        return false;
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
}

public struct InventorySlot
{
    public ItemSO item;
    public int currentAmount;

    public bool isFull => currentAmount >= item.MaxStackSize;
}
