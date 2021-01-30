using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamPickerController : MonoBehaviour {
	public List<TeamButton> teamButtons;
	public Button selectTeamButton;
	public TeamPanelController teamPanel;

	private void Awake() {
		var teams = GameManager.Instance.teams;
		for (int i = 0; i < teams.Count; i++) {
			this.teamButtons[i].Initialize(teams[i]);
		}

		this.selectTeamButton.onClick.AddListener(() => {
			GameManager.Instance.SelectTeam(this.teamPanel.CurrentTeam);
		});
	}

	private void Start() {
		
	}
	
	private void Update() {
		
	}
}
