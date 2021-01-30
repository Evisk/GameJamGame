﻿using System.Collections;
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

    public PanelState panelState;

    public TMP_Dropdown tacticDropDown;

    private void Awake() {

        tacticDropDown.options.Add(new TMP_Dropdown.OptionData(TeamTacticName.Aggresive.ToString()));
        tacticDropDown.options.Add(new TMP_Dropdown.OptionData(TeamTacticName.Defensive.ToString()));

        tacticDropDown.onValueChanged.AddListener(indexer => {
            CurrentTeam.teamTactic = (TeamTacticName)indexer;        
        });

        if (panelState == PanelState.Enemy)
        {
            this.CurrentTeam = GameManager.Instance.EnemyTeam;
            tacticDropDown.interactable = false;
        }
        else if (panelState == PanelState.Player)
        {
            tacticDropDown.interactable = false;
            this.CurrentTeam = GameManager.Instance.SelectedTeam;
        }
            
        this.LoadTeam(this.CurrentTeam);
	}

    public void LoadTeam(Team team)
    {
        this.CurrentTeam = team;
        TeamName.GetComponent<TextMeshProUGUI>().text = CurrentTeam.Name;
        Description.GetComponent<TextMeshProUGUI>().text = CurrentTeam.Description;

        tacticDropDown.value = (int)team.teamTactic;

        int index = 0;
        foreach (var statPair in team.mainStats) {
            this.MainStatistics[index].Initialize(statPair.statName, statPair.value, true);
            index++;
		}

        index = 0;
        foreach (var statPair in team.secondaryStats) {
            this.SecondaryStatistics[index].Initialize(statPair.statName, statPair.value, panelState != PanelState.Player);
            index++;
        }
    }



}
