using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Team")]
public class Team : ScriptableObject
{
    public string Name;
    [SerializeField]
    private StringIntDictionary MainStatistics = StringIntDictionary.New<StringIntDictionary>();
    public Dictionary<string, int> _mainStatistics
    {
        get { return MainStatistics.dictionary; }
    }
    [SerializeField]
    private StringIntDictionary SecondaryStatistics = StringIntDictionary.New<StringIntDictionary>();
    public Dictionary<string, int> _secondaryStatistics
    {
        get { return SecondaryStatistics.dictionary; }
    }
    [TextArea(3, 50)]
    public string Description;
    public Sprite CombatantSprite;


  
}
