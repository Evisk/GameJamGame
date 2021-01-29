using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamPanelController : MonoBehaviour
{
    public GameObject TeamName;
    public GameObject Description;
    public GameObject MainStatistics;
    public GameObject CombatantSprite;
    public Team CurrentTeam;

    public void SetTeam(Team team)
    {
        CurrentTeam = team;
    }

    public void LoadTeam()
    {
        TeamName.GetComponent<TextMeshProUGUI>().text = CurrentTeam.Name;
        Description.GetComponent<TextMeshProUGUI>().text = CurrentTeam.Description;

        foreach(Transform statistic in MainStatistics.transform)
        {

        }
    }



}
