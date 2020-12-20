using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Represents battle controller which controls battle.
/// </summary>
public class BattleController : MonoBehaviour {
    /// <summary>
    /// Enemies which will fight against player.
    /// </summary>
    public List<Character> Enemies { get; set; }
    
    /// <summary>
    /// Character, which will be used.
    /// </summary>
    public Character ChosenCharacter { get; set; }
    
    /// <summary>
    /// Targets on which skill will be used.
    /// </summary>
    public List<Character> ChosenTargets { get; private set; }
    
    /// <summary>
    /// Targets on which skill can be used.
    /// </summary>
    public List<Character> ValidTargets { get; private set; }
    
    /// <summary>
    /// Skill which will be used.
    /// </summary>
    public Skill ChosenSkill { get; set; }
    
    /// <summary>
    /// Flag which shows whether character was revived in current turn. If yes, then turn is ended.
    /// </summary>
    public bool CharacterRevived { get; set; }
    
    /// <summary>
    /// Flag used to signalize to character hover, that battle is ready and character events can be added.
    /// </summary>
    public bool battleReady;
    
    /// <summary>
    /// Flag used to signalize whether team skips turn. It is set to true when last unplayed character of team is
    /// stunned. When this happens, team should skip its turn since teams alternate turns.
    /// </summary>
    public bool SkipTurn { get; set; } = false;
    
    /// <summary>
    /// List of effects which have global influence, such as decreasing health of king as a result of successful event.
    /// </summary>
	public List<Effect> GlobalEffects { get; private set; } = new List<Effect>();

    private Team playerTeam;
    private Team enemyTeam;
    
    /// <summary>
    /// Scene controller from GameMaster GameObject.
    /// </summary>
	private SceneController sceneController;
    
    /// <summary>
    /// Team that has current turn.
    /// </summary>
    private Team currentTeam;
    
    /// <summary>
    /// Represents whether player team of enemy team has first turn in battle. Is generated at the beggining of each
    /// battle.
    /// </summary>
    private bool playerTeamFirst;
    
    /// <summary>
    /// UI element containing skills.
    /// </summary>
    private GameObject skillField;
    private Random rng;

    /// <summary>
    /// Queue of battle log.
    /// </summary>
    private Queue<String> logs;
    
    /// <summary>
    /// Helper used to manipulate more easily with battle log.
    /// </summary>
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

    public delegate void OnTurnEndDelegate(BattleController bc);
    
    /// <summary>
    /// Event which is emitted when turn ends.
    /// </summary>
    public event OnTurnEndDelegate onTurnEnd;

    /// <summary>
    /// Sets initial values of some attributes.
    /// </summary>
    private void Awake() {
        rng = new Random();
		sceneController = GameMaster.instance.GetComponent<SceneController>();
        logs = new Queue<string>();
        battleReady = false;
    }

    /// <summary>
    /// Instantiates Enemies and scales them to player level.
    /// </summary>
    private void FillEnemiesToBattle() {
        Vector3 position = enemyTeam.transform.position;

        foreach (Character enemy in Enemies) {
            Character newEnemy = Instantiate(enemy, position, Quaternion.identity, enemyTeam.transform);
            newEnemy.ScaleAICharacterToPlayerLevelAndArea(playerTeam.Level, playerTeam.CurrentArea);
            enemyTeam.AddCharacterToTeam(newEnemy);
            position[0] += 2.2f;
        }
    }

    /// <summary>
    /// Applies global effects.
    /// </summary>
    /// <param name="team">Team to which effects are applied.</param>
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

    /// <summary>
    /// Initializes player and enemy team. Also generates which team has first turn. Then starts Battle coroutine.
    /// </summary>
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

    /// <summary>
    /// Resets attributes after battle and returns from battle scene.
    /// </summary>
    /// <param name="isPlayerWinner">Boolean signalizing whether player won or lost.</param>
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

    /// <summary>
    /// Resets team at the end of each turn.
    /// </summary>
    /// <param name="team">Team to reset.</param>
    /// <param name="teamFinished">Boolean representing if team played all of its characters at least once.</param>
    /// <returns></returns>
    private bool ResetTeam(Team team, bool teamFinished) {
        if (team.UnplayedCharacters.Count == 0) {
            team.ResetTeam(false);
            return true;
        }
        // If team already finished, flag value is not changed
        return teamFinished;
    }

    /// <summary>
    /// Obtains valid target of ChosenSkill used by ChosenCharacter. If ChosenCharacter is confused or some of skill's
    /// valid targets are invisible, valid targets list might change.
    /// </summary>
    /// <param name="playerTeam">Player team.</param>
    /// <param name="enemyTeam">Enemy team.</param>
    /// <param name="playerTeamRound">Boolean representing whether player team has first turn.</param>
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

    /// <summary>
    /// Coroutine used to control battle.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Battle() {
        bool playerTeamRound = playerTeamFirst;
        // Battle lasts until one team loses all characters.
        while (playerTeam.Characters.Count > 0 && enemyTeam.Characters.Count > 0) {
            Log = "NEW ROUND";
            playerTeam.ResetTeam(true);
            enemyTeam.ResetTeam(true);

            bool playerTeamFinished = false;
            bool enemyTeamFinished = false;
            // Round lasts until both teams played each character at least once or until one team loses all characters.
            while ((!playerTeamFinished || !enemyTeamFinished) &&
				   (playerTeam.Characters.Count > 0 &&
				   enemyTeam.Characters.Count > 0)) {
                ChosenCharacter = null;

                currentTeam = (playerTeamRound) ? playerTeam: enemyTeam;
                Log = $"{((playerTeamRound) ? "Player" : "Enemy")}'s turn";

                // Selection of character.
                currentTeam.HighlightUnplayed();
                if (currentTeam.isAI)
                    currentTeam.SelectCharacter();
                while (ChosenCharacter == null) {
                    yield return new WaitForSeconds(0.5f);
                }

                // If some character was revived, turn ends.
                if (!CharacterRevived) {
                    
                    // Selection of skill.
                    ChosenSkill = null;
                    if (currentTeam.isAI)
                        ChosenCharacter.SelectSkill();
                    while (ChosenSkill == null) {
                        yield return new WaitForSeconds(0.5f);
                    }

                    GetTargets(playerTeam, enemyTeam, playerTeamRound);

                    // If skill has targets but none are valid (all targets have Invisibility, etc.), turn ends.
                    int numOfTargets = ChosenSkill.GetNumOfTargets();
                    if (ValidTargets.Count > 0 && numOfTargets != 0 || numOfTargets == 0) {
                        if (numOfTargets == -1)
                            numOfTargets = ValidTargets.Count;

                        // Selection of targets.
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

                // Teams are reset to allow smaller teams to continue playing until larger team is finished.
                playerTeamFinished = ResetTeam(playerTeam, playerTeamFinished);
                enemyTeamFinished = ResetTeam(enemyTeam, enemyTeamFinished);

                // Emit onTurnEnd event.
                TurnEnd();

                // If last unplayed character of opposing team has been stunned, that team will skip its round.
                if (!SkipTurn)
                    playerTeamRound = !playerTeamRound;
                SkipTurn = false;
                CharacterRevived = false;
            }

            // Apply effects for both teams.
            playerTeam.ApplyEffects();
            enemyTeam.ApplyEffects();
        }

        // End battle function call.
		AfterBattle(playerTeam.Characters.Count > 0);
    }

    /// <summary>
    /// Returns team that has current turn.
    /// </summary>
    /// <returns>Current team.</returns>
    public Team GetCurrentTeam() {
        return currentTeam;
    }

    /// <summary>
    /// Emits onTurnEnd event for Skills UI element.
    /// </summary>
    private void TurnEnd() {
        Log = "\n";
        onTurnEnd?.Invoke(this);
    }
}
