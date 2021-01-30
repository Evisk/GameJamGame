using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public GameObject brawlCloud;
    public GameObject throwCombatantsPlayer;
    public GameObject throwCombatantsEnemy;

    public List<GameObject> playerCombatants = new List<GameObject>();
    public List<GameObject> enemyCombatants = new List<GameObject>();
    public TMP_Text endBattleText;

    public List<Material> throwMaterials = new List<Material>();

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
        DontDestroyOnLoad(this);
    }

    public void SelectTeam(Team team) {
        this.SelectedTeam = Instantiate(team);
        this.EnemyTeams = teams.ToList();
        this.EnemyTeams.Remove(team);

        SelectRandomEnemyTeam();

        this.AwardAdditionalPoints();

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
        int enemy = UnityEngine.Random.Range(0, this.EnemyTeams.Count);
        this.EnemyTeam = Instantiate(this.EnemyTeams[enemy]);
        this.EnemyTeams.RemoveAt(enemy);

        RandomEnemySecondaries();

        UIManager.Instance.ShowPreFight();
    }

    public void RandomEnemySecondaries()
    {
        int numToDistribute = pointsPerRound * (currentRound + 1);

        for (int i = 0; i < numToDistribute; i++)
        {
            int newStat = UnityEngine.Random.Range(0, this.EnemyTeam.secondaryStats.Count);
            this.EnemyTeam.secondaryStats[newStat].value += 1;
        }
    }


    public void LoadTeamSprites()
    {
        float playerTeamFacingScale = GetFacingScaleAccordingToTeam(SelectedTeam.teamName,true);
        float enemyTeamFacingScale = GetFacingScaleAccordingToTeam(EnemyTeam.teamName, false);

        foreach (GameObject combatant in playerCombatants)
        {
            combatant.GetComponent<SpriteRenderer>().sprite = SelectedTeam.CombatantSprite;
            Vector3 newScale = combatant.transform.localScale;
            newScale.x = playerTeamFacingScale;
            combatant.transform.localScale = newScale;
        }

        foreach (GameObject combatant in enemyCombatants)
        {
            combatant.GetComponent<SpriteRenderer>().sprite = EnemyTeam.CombatantSprite;
            Vector3 newScale = combatant.transform.localScale;
            newScale.x = enemyTeamFacingScale;
            combatant.transform.localScale = newScale;
        }
    }

    public float GetFacingScaleAccordingToTeam(TeamName teamName,bool player)
    {

        float positive = .5f;
        float negative = -.5f;
        switch (teamName)
        {
            case TeamName.Aliens:
                return player ? positive : negative;

            case TeamName.FlockOfBirds:
                return player ? positive : negative;

            case TeamName.GameJam:
                return player ? positive : negative;

            case TeamName.PacMan:
                return player ? negative : positive;

            case TeamName.Pirates:
                return player ? negative : positive;

            default:
                return player ? negative : positive;
        }
    }

    public void ResetToStartPosition()
    {
        for(int i = 0; i < playerCombatants.Count; i++)
        {
            playerCombatants[i].transform.position = playerStartPositions[i].transform.position;
            enemyCombatants[i].transform.position = enemyStartPositions[i].transform.position;
        }
        this.endBattleText.text = "";
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
        LoadTeamSprites();
        ResetBattle();
        StartCoroutine(Brawl());
    }

    public void AwardAdditionalPoints()
    {
        availablePoints += pointsPerRound;
    }

    public Material GetThrowMaterial(TeamName team)
    {
        switch (team)
        {
            case TeamName.Aliens:
                return throwMaterials[0];
            case TeamName.FlockOfBirds:
                return throwMaterials[1];
            case TeamName.GameJam:
                return throwMaterials[2];
            case TeamName.PacMan:
                return throwMaterials[3];
            default:
                return throwMaterials[4];
        }
    }

    public IEnumerator Brawl()
    {
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(MoveToCombatPositionsRoutine(2.5f));       
        yield return StartCoroutine(SpecialMoveRoutine(.75f));
        brawlCloud.GetComponent<ParticleSystem>().Play();
        throwCombatantsPlayer.GetComponent<ParticleSystemRenderer>().material = GetThrowMaterial(SelectedTeam.teamName);
        throwCombatantsEnemy.GetComponent<ParticleSystemRenderer>().material = GetThrowMaterial(EnemyTeam.teamName);
        throwCombatantsPlayer.GetComponent<ParticleSystem>().Play();
        throwCombatantsEnemy.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(10f);

        // show the brawl

        var playerPoints = 100; //this.GetCombatPoints(this.SelectedTeam, this.EnemyTeam.teamName);
        var enemyPoints = this.GetCombatPoints(this.EnemyTeam, this.SelectedTeam.teamName);

        Debug.Log($"Player Points: {playerPoints:F2}, Enemy Points: {enemyPoints:F2}");

        var win = playerPoints > enemyPoints;
        if (win) {
            this.endBattleText.text = "Victory!";
		} else {
            this.endBattleText.text = "Defeat!";
        }

        yield return new WaitForSeconds(2f);

        battleField.SetActive(false);

        if (win) {

            if (this.currentRound == 2)
            {
                UIManager.Instance.ShowVictory();
            }
            else
            {
                this.currentRound++;
                this.LockPoints();
                this.AwardAdditionalPoints();
                this.SelectRandomEnemyTeam();
            }

                      
        } else {
            UIManager.Instance.ShowTeams();
            this.ResetGame();
        }
    }

	private void LockPoints() {
        foreach (var stat in this.SelectedTeam.secondaryStats) {
            stat.lockedValue = stat.value;
		}
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

    public IEnumerator SpecialMoveRoutine(float totalTime)
    {
        var dt = 0f;
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

    public float GetCombatPoints(Team team, TeamName enemyTeam) {
        var mainPoints = team.mainStats.Select(st => st.value).Sum();

        var secondaryPoints = team.secondaryStats.Select(st => this.GetStatMultiplier(st.statName, enemyTeam) * st.value).Sum();

        var result = mainPoints + secondaryPoints;

        if (team.teamTactic == TeamTacticName.Aggressive) {
            result += UnityEngine.Random.Range(-6f, 6f);
		} else {
            result += UnityEngine.Random.Range(-3f, 3f);
            result += 1f;
        }

        return result;
	}

	private void ResetGame() {
        this.currentRound = 0;
        this.availablePoints = 0;
    }

    private void ResetBattle() {
        this.ResetToStartPosition();
        this.endBattleText.text = "";
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
    Aggressive,
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
