using UnityEngine;

public abstract class ItemEffectSO : ScriptableObject
{
    public abstract bool ExecuteEffect(GameObject target, ItemSO sourceItem);
}
