using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InventorySaveManager
{
    public List<SlotSaveData> slotsData = new List<SlotSaveData>();
}

[System.Serializable]
public struct SlotSaveData
{
    public int itemID;
    public int itemAmount;
    public int slotIndex;

    public SlotSaveData(int _itemID, int _itemAmount, int _slotIndex)
    {
        itemID = _itemID;
        itemAmount = _itemAmount;
        slotIndex = _slotIndex;
    }
}
