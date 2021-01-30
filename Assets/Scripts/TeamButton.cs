using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamButton : MonoBehaviour {
	public Team team;
	public Image avatar;
	public TMP_Text teamName;
	public TeamPanelController teamPanel;

	private void Awake() {
		this.GetComponent<Button>().onClick.AddListener(() => {
			this.teamPanel.LoadTeam(this.team);
		});
	}

	internal void Initialize(Team team) {
		this.team = team;
		this.teamName.text = team.Name;
		this.avatar.sprite = team.CombatantSprite;
	}
}
