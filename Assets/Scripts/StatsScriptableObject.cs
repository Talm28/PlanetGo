using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    MaxHealth,
    Strength,
    Speed,
    LeftSpawnAngle,
    RightSpawnAngle,
    MinSize,
    MaxSize,
    MaxMovementTick,
    MinMovementTick,
    TriggeredDistance,
    CoinsValue,
    ScoreValue
}


[CreateAssetMenu(fileName = "Stats")]
public class StatsScriptableObject : ScriptableObject
{
    [System.Serializable] // Class for one stat item. Using for inspector show selution
    public class StatItem
    {
        public Stat stat;
        public float value;
    }

    [SerializeField]
    private StatItem[] stats;

    public float GetItem(string statName)
    {
        foreach(StatItem stat in stats)
        {
            if(stat.stat.ToString() == statName )
                return stat.value;
        }
        return -1;
    }
}
