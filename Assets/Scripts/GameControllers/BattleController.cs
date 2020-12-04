using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BattleController : MonoBehaviour {
    private List<Character> enemies;
    public List<Character> Enemies { get; set; }
    private GameObject playerTeam;
    private GameObject enemyTeam;
    public Character ChosenCharacter { get; set; }
    public List<Character> ChosenTargets { get; private set; }

    public List<Character> ValidTargets { get; private set; }
    public Skill ChosenSkill { get; set; }

    private Team currentTeam;
    private bool playerTeamFirst;
    private GameObject skillField;

    private Random rng;

    private void Awake() {
        rng = new Random();
    }

    public void FillEnemiesToBattle() {
        enemyTeam = GameObject.FindGameObjectWithTag("EnemyTeam");
        Team enemyTeamComp = enemyTeam.GetComponent<Team>();
        Transform enemyTeamTransform = enemyTeam.transform;
        Vector3 position = enemyTeamTransform.position;

        foreach (Character enemy in Enemies) {
            Character newEnemy = Instantiate(enemy, position, Quaternion.identity, enemyTeamTransform);
            enemyTeamComp.AddCharacterToTeam(newEnemy);
            newEnemy.SetTeam(enemyTeamComp);
            position[0] += 1;
        }
    }

    public void InitializeCombat() {
        playerTeam = GameObject.FindGameObjectWithTag("PlayerTeam");
        playerTeam.transform.position = new Vector3(-5, 0, 0);
        playerTeam.GetComponent<CharacterMovement>().enabled = false;

        playerTeamFirst = (rng.Next(2) == 0);

        Team playerTeamComp = (Team) playerTeam.GetComponent(typeof(Team));
        playerTeamComp.BattleController = this;
        foreach (Character c in playerTeamComp.Characters) {
            c.InitializeForBattle(this);
        }

        Team enemyTeamComp = (Team) enemyTeam.GetComponent(typeof(Team));
        enemyTeamComp.BattleController = this;
        foreach (Character c in ((Team) enemyTeam.GetComponent(typeof(Team))).Characters) {
            c.InitializeForBattle(this);
        }

        skillField = GameObject.FindGameObjectWithTag("SkillField");
        foreach (UISkillField field in skillField.transform.GetComponentsInChildren<UISkillField>()) {
            field.SetBattleController(this);
        }

        StartCoroutine(Combat());
    }

    private IEnumerator Combat() {
        Team playerTeamComp = (Team) playerTeam.GetComponent(typeof(Team));
        Team enemyTeamComp = (Team) enemyTeam.GetComponent(typeof(Team));

        while (playerTeamComp.Characters.Count > 0 && enemyTeamComp.Characters.Count > 0) {
            Debug.Log("Nove kolo zacalo");
            foreach (Character c in playerTeamComp.Characters) {
                playerTeamComp.AddUnplayedCharacter(c);
                playerTeamComp.RemovePlayedCharacter(c); // prerobit tak, aby to vymazalo aj mrtve postavy
            }

            foreach (Character c in enemyTeamComp.Characters) {
                enemyTeamComp.AddUnplayedCharacter(c);
                enemyTeamComp.RemovePlayedCharacter(c);
            }

            bool playerTeamRound = playerTeamFirst;
            while (playerTeamComp.UnplayedCharacters.Count > 0 ||
                   enemyTeamComp.UnplayedCharacters.Count > 0) {
                Debug.Log("Novy tah");
                ChosenCharacter = null;

                currentTeam = (playerTeamRound) ? playerTeamComp : enemyTeamComp;
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
                ValidTargets = ChosenSkill.HighlightTargets(playerTeamComp, enemyTeamComp, playerTeamRound);
                
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