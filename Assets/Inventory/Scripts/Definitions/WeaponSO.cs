using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem_", menuName = "Inventory System/Item/Weapon")]
public class WeaponSO : ItemSO
{
    [SerializeField] private float _damage;
}
