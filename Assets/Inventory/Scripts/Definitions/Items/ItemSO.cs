using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Item_", menuName = "Inventory System/Item/New Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] protected List<ItemEffectSO> effectsList = new List<ItemEffectSO>();
    [SerializeField] private int _itemID;
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private int _maxStackSize = 1;
    [SerializeField] private string _itemDesc;

    public int ItemID       => _itemID;
    public string ItemName  => _itemName;
    public Sprite ItemIcon  => _itemIcon;
    public int MaxStackSize => _maxStackSize;
    public string ItemDesc  => _itemDesc;

    public bool CanUseItem(GameObject target)
    {
        bool anyEffectApplied = false;
        foreach(var effect in effectsList)
        {
            bool isApplied = effect.ExecuteEffect(target, this);

            if(isApplied)
            {
                anyEffectApplied = true;
            }
        }
        return anyEffectApplied;
    }
}
