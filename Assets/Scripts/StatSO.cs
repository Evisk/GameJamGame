using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatSO : ScriptableObject {
	public StatisticName stat;
	public List<Effectiveness> effectiveness;

	public float GetMultiplier(TeamName team) {
		return effectiveness.Where(eff => eff.team == team).Single().effectiveness / 100f;
	}

	[Serializable]
	public class Effectiveness {
		public TeamName team;
		public int effectiveness;
	}
}
