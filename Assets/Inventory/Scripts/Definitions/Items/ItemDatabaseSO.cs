using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase_", menuName = "Inventory System/Item/Database/New Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();
    private Dictionary<int, ItemSO> itemDict;

    public void InitializeDatabase()
    {
        if(itemDict != null) return;

        itemDict = new Dictionary<int, ItemSO>();
        foreach(var item in items)
        {
            if(item != null && !itemDict.ContainsKey(item.ItemID))
            {
                itemDict.Add(item.ItemID, item);
            }
        }
    }

    public ItemSO GetItemFromDatabase(int itemID)
    {
        InitializeDatabase();
        
        if(itemDict.TryGetValue(itemID, out ItemSO item))
        {
            return item;
        }
        return null;
    }
}
