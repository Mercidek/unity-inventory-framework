using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem_", menuName = "Inventory System/Item/Consumable")]
public class ConsumableSO : ItemSO
{
    [SerializeField] private float _healAmount;
}
