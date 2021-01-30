using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public List<Team> teams = new List<Team>();

    public Team SelectedTeam;
    public List<Team> EnemyTeams;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));

                if(_instance == null)
                {
                    GameObject sObject = new GameObject();
                    _instance = sObject.AddComponent<GameManager>();
                    sObject.name = "GameManager";
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SelectTeam(Team team) {
        this.SelectedTeam = team;
        this.EnemyTeams = this.teams.ToList();
        this.EnemyTeams.Remove(team);

        Debug.Log($"Picked {team.Name}");
	}

    public void IncreaseStatistic(string name)
    {
        if(SelectedTeam._secondaryStatistics[name] > 0)
            SelectedTeam._secondaryStatistics[name]--;
    }

    public void DecreaseStatistic(string name)
    {
        if (SelectedTeam._secondaryStatistics[name] < 10)
            SelectedTeam._secondaryStatistics[name]++;
    }


    public void Exit()
    {
        Application.Quit();
    }
}
