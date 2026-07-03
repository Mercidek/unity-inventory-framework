using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Inventory System/New Item")]
public class ItemType : ScriptableObject
{
    [SerializeField] private int _itemID;
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private int _maxStackSize = 1;
}
