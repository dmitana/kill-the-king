using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class BattleController : MonoBehaviour {
    public List<Character> Enemies { get; set; }
    public Character ChosenCharacter { get; set; }
    public List<Character> ChosenTargets { get; private set; }
    public List<Character> ValidTargets { get; private set; }
    public Skill ChosenSkill { get; set; }
    public bool CharacterRevived { get; set; }
    public bool battleReady;
    public bool SkipTurn { get; set; } = false;
	public List<Effect> GlobalEffects { get; private set; } = new List<Effect>();

    private Team playerTeam;
    private Team enemyTeam;
	private SceneController sceneController;
    private Team currentTeam;
    private bool playerTeamFirst;
    private GameObject skillField;
    private Random rng;

    private Queue<String> logs;
    public String Log {
        get {
            if (logs.Count == 0)
                return null;
            return logs.Dequeue();
        }

        set {
            logs.Enqueue(value);
        }
    }

    public delegate void OnClickDelegate(BattleController bc);
    public event OnClickDelegate onTurnEnd;

    private void Awake() {
        rng = new Random();
		sceneController = GameMaster.instance.GetComponent<SceneController>();
        logs = new Queue<string>();
        battleReady = false;
    }

    private void FillEnemiesToBattle() {
        Vector3 position = enemyTeam.transform.position;

        foreach (Character enemy in Enemies) {
            Character newEnemy = Instantiate(enemy, position, Quaternion.identity, enemyTeam.transform);
            newEnemy.ScaleAICharacterToPlayerLevelAndArea(playerTeam.Level, playerTeam.CurrentArea);
            enemyTeam.AddCharacterToTeam(newEnemy);
            position[0] += 2.2f;
        }
    }

	private void ApplyGlobalEffects(Team team) {
		bool isApplied = false;

		for (int i = GlobalEffects.Count - 1; i >= 0; --i) {
			isApplied = GlobalEffects[i].GlobalApply(team);
			if (isApplied) {
				Destroy(GlobalEffects[i].gameObject);
				GlobalEffects.RemoveAt(i);
			}
		}
	}

    public void InitializeBattle() {
		GameObject playerTeamInitialPosition = GameObject.FindGameObjectWithTag("PlayerTeamInitialPosition");
		playerTeam = Team.playerTeamInstance;
		playerTeam.InitilizeForBattle(playerTeamInitialPosition.transform.position);

        enemyTeam = GameObject.FindGameObjectWithTag("EnemyTeam").GetComponent<Team>();
		FillEnemiesToBattle();
		enemyTeam.InitilizeForBattle();

        playerTeamFirst = (rng.Next(2) == 0);

        skillField = GameObject.FindGameObjectWithTag("SkillField");

        ApplyGlobalEffects(enemyTeam);

        StartCoroutine(Battle());
        battleReady = true;
    }

	private void AfterBattle(bool isPlayerWinner) {
        logs = new Queue<string>();
        ChosenCharacter = null;
        ChosenSkill = null;
        ChosenTargets = null;
        ValidTargets = null;

        if (isPlayerWinner) {
			playerTeam.AfterBattle();
			sceneController.ReturnFromBattleScene();
		}
		else
			sceneController.EndGameLoss();

        battleReady = false;
    }

    private bool ResetTeam(Team team, bool teamFinished) {
        if (team.UnplayedCharacters.Count == 0) {
            team.ResetTeam(false);
            return true;
        }
        // If team already finished, flag value is not changed
        return teamFinished;
    }

    private void GetTargets(Team playerTeam, Team enemyTeam, bool playerTeamRound) {
        ChosenTargets = new List<Character>();

        ConfusionDebuff debuff = 
            (ConfusionDebuff) ChosenCharacter.GetEffects().Find(e => e.GetType() == typeof(ConfusionDebuff));
        if (debuff != null && debuff.duration == 1) {
            playerTeamRound = !playerTeamRound;
            debuff.Activate(ChosenCharacter, ChosenSkill);
        }

        ValidTargets = ChosenSkill.HighlightTargets(playerTeam, enemyTeam, playerTeamRound)
            .FindAll(c => !c.IsInvisible);

        if (ValidTargets.Count > 0)
            Log = $"Valid targets: {string.Join(", ", ValidTargets)}";
        else if (ChosenSkill.numOfTargets == 0)
            Log = "This skill is applied on character that uses it";
        else
            Log = "No valid targets found";
    }

    private IEnumerator Battle() {
        bool playerTeamRound = playerTeamFirst;
        while (playerTeam.Characters.Count > 0 && enemyTeam.Characters.Count > 0) {
            Log = "NEW ROUND";
            playerTeam.ResetTeam(true);
            enemyTeam.ResetTeam(true);

            bool playerTeamFinished = false;
            bool enemyTeamFinished = false;
            while ((!playerTeamFinished || !enemyTeamFinished) &&
				   (playerTeam.Characters.Count > 0 &&
				   enemyTeam.Characters.Count > 0)) {
                ChosenCharacter = null;

                currentTeam = (playerTeamRound) ? playerTeam: enemyTeam;
                Log = $"{((playerTeamRound) ? "Player" : "Enemy")}'s turn";

                currentTeam.HighlightUnplayed();
                if (currentTeam.isAI)
                    currentTeam.SelectCharacter();
                while (ChosenCharacter == null) {
                    yield return new WaitForSeconds(0.5f);
                }

                if (!CharacterRevived) {
                    ChosenSkill = null;
                    if (currentTeam.isAI)
                        ChosenCharacter.SelectSkill();
                    while (ChosenSkill == null) {
                        yield return new WaitForSeconds(0.5f);
                    }

                    GetTargets(playerTeam, enemyTeam, playerTeamRound);

                    int numOfTargets = ChosenSkill.GetNumOfTargets();
                    if (ValidTargets.Count > 0 && numOfTargets != 0 || numOfTargets == 0) {
                        if (numOfTargets == -1)
                            numOfTargets = ValidTargets.Count;

                        if (currentTeam.isAI)
                            currentTeam.SelectTargets(numOfTargets, ValidTargets);
                        while (numOfTargets != ChosenTargets.Count) {
                            yield return new WaitForSeconds(0.5f);
                        }

                        ChosenSkill.ApplySkill(ChosenCharacter, ChosenTargets);
                        currentTeam.AddPlayedCharacter(ChosenCharacter);
                        currentTeam.RemoveUnplayedCharacter(ChosenCharacter);
                    }
                }

                playerTeamFinished = ResetTeam(playerTeam, playerTeamFinished);
                enemyTeamFinished = ResetTeam(enemyTeam, enemyTeamFinished);

                TurnEnd();

                if (!SkipTurn)
                    playerTeamRound = !playerTeamRound;
                SkipTurn = false;
                CharacterRevived = false;
            }

            // Apply effects
            playerTeam.ApplyEffects();
            enemyTeam.ApplyEffects();
        }

        // End battle function call
		AfterBattle(playerTeam.Characters.Count > 0);
    }

    public Team GetCurrentTeam() {
        return currentTeam;
    }

    public GameObject GetSkillField() {
        return skillField;
    }

    private void TurnEnd() {
        Log = "\n";
        onTurnEnd?.Invoke(this);
    }
}
