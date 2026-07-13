using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private StatType statType;
    [SerializeField] private float currentValue;
    [SerializeField] private float maxValue;

    public StatType Type => statType;
    public float CurrentValue => currentValue;
    public float MaxValue => maxValue;

    public bool ChangeValue(float value)
    {
        if((currentValue >= maxValue && value > 0) || (currentValue <= 0 && value < 0))
        {
            return false;
        }
        
        currentValue += value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        return true;
    }
}
