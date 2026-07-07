using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Inventory System/Item/Generic")]
public class ItemSO : ScriptableObject
{
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
}
