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

    private void Awake() {
        rng = new Random();
		sceneController = GameMaster.instance.GetComponent<SceneController>();
        logs = new Queue<string>();
    }

    private void FillEnemiesToBattle() {
        Vector3 position = enemyTeam.transform.position;

        foreach (Character enemy in Enemies) {
            Character newEnemy = Instantiate(enemy, position, Quaternion.identity, enemyTeam.transform);
            enemyTeam.AddCharacterToTeam(newEnemy);
            position[0] += 1;
        }
    }

    public void InitializeBattle() {
		playerTeam = Team.playerTeamInstance;
		playerTeam.InitilizeForBattle(this, new Vector3(-5, 0, 0));

        enemyTeam = GameObject.FindGameObjectWithTag("EnemyTeam").GetComponent<Team>();
		FillEnemiesToBattle();
		enemyTeam.InitilizeForBattle(this);

        playerTeamFirst = (rng.Next(2) == 0);

        skillField = GameObject.FindGameObjectWithTag("SkillField");
        foreach (UISkillField field in skillField.transform.GetComponentsInChildren<UISkillField>()) {
            field.SetBattleController(this);
        }

        StartCoroutine(Battle());
    }

	private void AfterBattle() {
		playerTeam.AfterBattle();
		sceneController.ReturnFromBattleScene();
	}

    private void ClearSkillField() {
        UISkillField[] skillFields = skillField.transform.GetComponentsInChildren<UISkillField>();
        foreach (UISkillField skillField in skillFields) {
            skillField.Clear();
        }
    }

    private bool ResetTeam(Team team, bool teamFinished) {
        if (team.UnplayedCharacters.Count == 0) {
            team.ResetTeam(false);
            return true;
        }
        // If team already finished, flag value is not changed
        return teamFinished;
    }

    private IEnumerator Battle() {
        bool playerTeamRound = playerTeamFirst;
        while (playerTeam.Characters.Count > 0 && enemyTeam.Characters.Count > 0) {
            Log = "Nove kolo zacalo";
            playerTeam.ResetTeam(true);
            enemyTeam.ResetTeam(true);

            bool playerTeamFinished = false;
            bool enemyTeamFinished = false;
            while ((!playerTeamFinished || !enemyTeamFinished) &&
				   (playerTeam.Characters.Count > 0 &&
				   enemyTeam.Characters.Count > 0)) {
                Log = "Novy tah";
                ChosenCharacter = null;

                currentTeam = (playerTeamRound) ? playerTeam: enemyTeam;

                currentTeam.HighlightUnplayed();
                if (currentTeam.isAI)
                    currentTeam.SelectCharacter();
                while (ChosenCharacter == null) {
                    yield return new WaitForSeconds(0.5f);
                }

                ChosenSkill = null;
                ChosenCharacter.DisplaySkills();
                if (currentTeam.isAI)
                    ChosenCharacter.SelectSkill();
                while (ChosenSkill == null) {
                    yield return new WaitForSeconds(0.5f);
                }

                ChosenTargets = new List<Character>();
                ValidTargets = ChosenSkill.HighlightTargets(playerTeam, enemyTeam, playerTeamRound);

                int numOfTargets = ChosenSkill.GetNumOfTargets();
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

                playerTeamFinished = ResetTeam(playerTeam, playerTeamFinished);
                enemyTeamFinished = ResetTeam(enemyTeam, enemyTeamFinished);
                
                ClearSkillField();
                playerTeamRound = !playerTeamRound;
            }
        }

        // End battle function call
		AfterBattle();
    }

    public Team GetCurrentTeam() {
        return currentTeam;
    }

    public GameObject GetSkillField() {
        return skillField;
    }
}
