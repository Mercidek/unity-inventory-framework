using UnityEngine;
using System.Collections.Generic;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private List<Stat> statsList = new List<Stat>();
    private Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    public void Awake()
    {
        foreach(var stat in statsList)
        {
            stats[stat.Type] = stat;
        }
    }

    public bool ModifyStat(StatType statType, float amount)
    {
        if(stats.TryGetValue(statType, out var stat))
        {
            return stat.ChangeValue(amount);
        }
        return false;
    }
}
