using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Inventory System/Item/Generic")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private int _itemID;
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private int _maxStackSize = 1;
}
