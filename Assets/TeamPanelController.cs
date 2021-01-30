using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamPanelController : MonoBehaviour
{
    public GameObject TeamName;
    public GameObject Description;
    //public GameObject MainStatistics;
    public GameObject CombatantSprite;
    public Team CurrentTeam;

    public StatisticUI[] MainStatistics;
    public StatisticUI[] SecondaryStatistics;

    private void Awake() {
        this.LoadTeam(this.CurrentTeam);
	}

    public void LoadTeam(Team team)
    {
        this.CurrentTeam = team;
        TeamName.GetComponent<TextMeshProUGUI>().text = CurrentTeam.Name;
        Description.GetComponent<TextMeshProUGUI>().text = CurrentTeam.Description;

        int index = 0;
        foreach (var statPair in team._mainStatistics) {
            this.MainStatistics[index].Initialize(statPair.Key, statPair.Value, true);
            index++;
		}

        index = 0;
        foreach (var statPair in team._secondaryStatistics) {
            this.SecondaryStatistics[index].Initialize(statPair.Key, statPair.Value, false);
            index++;
        }

        //foreach(Transform statistic in MainStatistics.transform)
        //{

        //}
    }



}
