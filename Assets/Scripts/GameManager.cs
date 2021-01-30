using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.Collections.Generic.Dictionary<string, int>;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public List<Team> teams = new List<Team>();

    public Team SelectedTeam;
    public Team EnemyTeam;
    public List<Team> EnemyTeams;

    public List<GameObject> playerStartPositions = new List<GameObject>();
    public List<GameObject> playerBattlePositions = new List<GameObject>();
    public List<GameObject> enemyStartPositions = new List<GameObject>();
    public List<GameObject> enemyBattlePositions = new List<GameObject>();
    public GameObject pointOfBattle;
    public GameObject battleField;

    public List<GameObject> playerCombatants = new List<GameObject>();
    public List<GameObject> enemyCombatants = new List<GameObject>();

    public List<StatSO> stats = new List<StatSO>();

    public int availablePoints;
    public int currentRound = 0;

    public const int pointsPerRound = 2;

    public bool HasAvailablePoints { get => availablePoints > 0; }

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
        availablePoints = pointsPerRound;
        DontDestroyOnLoad(this);
    }

    public void SelectTeam(Team team) {
        this.SelectedTeam = Instantiate(team);
        this.EnemyTeams = teams.ToList();
        this.EnemyTeams.Remove(team);

        SelectRandomEnemyTeam();


        UIManager.Instance.ShowPreFight();        

        Debug.Log($"Picked {team.Name}");
	}


    public bool DecreaseStatistic(StatisticName name)
    {
        StatData statData = this.SelectedTeam.GetSecondaryStatData(name);
        int newStat = statData.value - 1;

        if (statData.value > 0 && newStat >= statData.lockedValue)
        {
            statData.value = newStat;
            availablePoints++;
            return true;
        }

        return false;
    }

    public bool IncreaseStatistic(StatisticName name)
    {
        StatData statData = this.SelectedTeam.GetSecondaryStatData(name);
        if (statData.value < 5 && HasAvailablePoints)
        {
            statData.value++;
            availablePoints--;
            return true;
        }

        return false;        
    }

   public void ResetRound()
    {
        currentRound = 0;
    }

    public bool IncreaseRound()
    {
        if(currentRound < 2)
        {
            currentRound++;
            return true;
        }

        return false;           
    }

    public void SelectRandomEnemyTeam()
    {
        int enemy = Random.Range(0, this.EnemyTeams.Count);
        this.EnemyTeam = Instantiate(this.EnemyTeams[enemy]);
        this.EnemyTeams.RemoveAt(enemy);

        RandomEnemySecondaries();
    }

    public void RandomEnemySecondaries()
    {
        int numToDistribute = pointsPerRound * (currentRound + 1);

        for (int i = 0; i < numToDistribute; i++)
        {
            int newStat = Random.Range(0, this.EnemyTeam.secondaryStats.Count);
            this.EnemyTeam.secondaryStats[newStat].value += 1;
        }
    }


    public void LoadTeamSprites()
    {
        foreach(GameObject combatant in playerCombatants)
        {
            combatant.GetComponent<SpriteRenderer>().sprite = SelectedTeam.CombatantSprite;
        }

        foreach (GameObject combatant in enemyCombatants)
        {
            combatant.GetComponent<SpriteRenderer>().sprite = EnemyTeam.CombatantSprite;
        }
    }

    public void ResetToStartPosition()
    {
        for(int i = 0; i < playerCombatants.Count; i++)
        {
            playerCombatants[i].transform.position = playerStartPositions[i].transform.position;
            enemyCombatants[i].transform.position = enemyStartPositions[i].transform.position;
        }
    }

    public void MoveToCombatPositions()
    {
        for (int i = 0; i < playerCombatants.Count; i++)
        {
            playerCombatants[i].transform.position = Vector3.MoveTowards(playerCombatants[i].transform.position, playerBattlePositions[i].transform.position, 10f * Time.deltaTime);
            enemyCombatants[i].transform.position = Vector3.MoveTowards(enemyCombatants[i].transform.position, enemyBattlePositions[i].transform.position, 10f * Time.deltaTime);
        }
    }

    public void SpecialMove()
    {
        for (int i = 0; i < playerCombatants.Count; i++)
        {
            playerCombatants[i].transform.position = Vector3.MoveTowards(playerCombatants[i].transform.position, pointOfBattle.transform.position, 10f * Time.deltaTime);
            enemyCombatants[i].transform.position = Vector3.MoveTowards(enemyCombatants[i].transform.position, pointOfBattle.transform.position, 10f * Time.deltaTime);
        }
    }

    public void StartBattle()
    {
        UIManager.Instance.ChangePreFight(false);
        battleField.SetActive(true);
       // LoadTeamSprites();
        ResetToStartPosition();
        StartCoroutine(Brawl());
    }

    public void AwardAdditionalPoints()
    {
        availablePoints += pointsPerRound;
    }

    public IEnumerator Brawl()
    {
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(MoveToCombatPositionsRoutine(2.5f));       
        StartCoroutine(SpecialMoveRoutine());
    }

    public IEnumerator MoveToCombatPositionsRoutine(float totalTime)
    {
        var dt = 0f;
        while (dt < totalTime)
        {
            dt += Time.deltaTime;
            MoveToCombatPositions();
            yield return null;
        }
    }

    public IEnumerator SpecialMoveRoutine()
    {
        var dt = 0f;
        var totalTime = 5f;
        while (dt < totalTime)
        {
            dt += Time.deltaTime;
            SpecialMove();
            yield return null;
        }
    }


    public void Exit()
    {
        Application.Quit();
    }

    public float GetStatMultiplier(StatisticName stat, TeamName enemyTeam) {
        return this.stats.Where(st => st.stat == stat).Single().GetMultiplier(enemyTeam);
	}
}



public enum StatisticName
{
    Teamwork,
    Skill,
    Decisions,
    Creativity,
    PMMazeArcadePower,
    PMStealth,
    PMAppetite,
    GJGadgetWidgets,
    GJInspiration,
    GJHackerTools,
    FBNaturalMiniBombs,
    FBLeadership,
    FBNavigation,
    ALCowRustling,
    ALCropCircleDrawing,
    ALGlutimusMaximusExploration,
    PPirateCodex,
    PWeaponry,
    PPillagingLooting
}

public enum TeamTacticName
{
    Aggresive,
    Defensive
}

public enum TeamName
{
    Aliens,
    FlockOfBirds,
    GameJam,
    PacMan,
    Pirates
}

public enum PanelState
{
    Pick,
    Enemy,
    Player
}
