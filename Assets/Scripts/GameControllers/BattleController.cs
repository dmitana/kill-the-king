using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake() {
        rng = new Random();
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
        playerTeam.transform.position = new Vector3(-5, 0, 0);
        playerTeamFirst = (rng.Next(2) == 0);

        enemyTeam = GameObject.FindGameObjectWithTag("EnemyTeam").GetComponent<Team>();
		FillEnemiesToBattle();

		playerTeam.InitilizeForBattle(this);
		enemyTeam.InitilizeForBattle(this);

        skillField = GameObject.FindGameObjectWithTag("SkillField");
        foreach (UISkillField field in skillField.transform.GetComponentsInChildren<UISkillField>()) {
            field.SetBattleController(this);
        }

        StartCoroutine(Battle());
    }

    private IEnumerator Combat() {
        Team playerTeamComp = (Team) playerTeam.GetComponent(typeof(Team));
        Team enemyTeamComp = (Team) enemyTeam.GetComponent(typeof(Team));

    private IEnumerator Battle() {
        while (playerTeam.Characters.Count > 0 && enemyTeam.Characters.Count > 0) {
            Debug.Log("Nove kolo zacalo");
            foreach (Character c in playerTeam.Characters) {
                playerTeam.AddUnplayedCharacter(c);
                playerTeam.RemovePlayedCharacter(c); // prerobit tak, aby to vymazalo aj mrtve postavy
            }

            foreach (Character c in enemyTeam.Characters) {
                enemyTeam.AddUnplayedCharacter(c);
                enemyTeam.RemovePlayedCharacter(c);
            }

            bool playerTeamRound = playerTeamFirst;
            while (playerTeam.UnplayedCharacters.Count > 0 ||
                   enemyTeam.UnplayedCharacters.Count > 0) {
                Debug.Log("Novy tah");
                ChosenCharacter = null;

                currentTeam = (playerTeamRound) ? playerTeam: enemyTeam;
                if (currentTeam.UnplayedCharacters.Count == 0) {
                    playerTeamRound = !playerTeamRound;
                    continue;
                }

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
                playerTeamRound = !playerTeamRound;
            }
        }

        // End battle function call
    }

    public Team GetCurrentTeam() {
        return currentTeam;
    }

    public GameObject GetSkillField() {
        return skillField;
    }
}
