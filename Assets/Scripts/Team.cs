using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName ="Team")]
public class Team : ScriptableObject
{
    public string Name;

    public TeamName teamName;

    [SerializeField]
    public List<StatData> mainStats = new List<StatData>();

    [SerializeField]
    public List<StatData> secondaryStats = new List<StatData>();

    [TextArea(3, 50)]
    public string Description;
    public Sprite CombatantSprite;
    public TeamTacticName teamTactic = TeamTacticName.Defensive;


    public bool IsStatisticLocked(StatisticName name,int newvalue)
    {
        if (newvalue < GetSecondaryStatData(name).value)
            return true;

        return false;
    }

    public StatData GetSecondaryStatData(StatisticName name)
    {
        return secondaryStats.Where(e => e.statName == name).FirstOrDefault();
    }

}
