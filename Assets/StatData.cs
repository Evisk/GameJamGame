using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatData
{
    public StatisticName statName;
    public int value;
    [NonSerialized]
    public int lockedValue;
}
