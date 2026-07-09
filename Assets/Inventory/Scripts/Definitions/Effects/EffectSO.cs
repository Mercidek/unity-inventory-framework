using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect_", menuName = "Inventory System/Item/Effect/New Effect")]
public class Effect : ItemEffectSO
{
    [SerializeField] private StatType statType = StatType.Health;
    [SerializeField] private float changeAmount = 10f;

    public override bool ExecuteEffect(GameObject target, ItemSO sourceItem)
    {
        if(target.TryGetComponent<StatsManager>(out var manager))
        {
            return manager.ModifyStat(statType, changeAmount);
        }
        return false;
    }
}
