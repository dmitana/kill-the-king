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
    private Character chosenCharacter;
    private List<Character> chosenTargets;
    private Skill chosenSkill;
    
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

        foreach (Character c in ((Team) playerTeam.GetComponent(typeof(Team))).GetCharacters()) {
            c.InitializeForBattle(this);
        }
        
        foreach (Character c in ((Team) enemyTeam.GetComponent(typeof(Team))).GetCharacters()) {
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

        while (playerTeamComp.GetCharacters().Count > 0 && enemyTeamComp.GetCharacters().Count > 0) {
            Debug.Log("Nove kolo zacalo");
            foreach (Character c in playerTeamComp.GetCharacters()) {
                playerTeamComp.AddUnplayedCharacter(c);
                playerTeamComp.RemovePlayedCharacter(c); // prerobit tak, aby to vymazalo aj mrtve postavy
            }
            
            foreach (Character c in enemyTeamComp.GetCharacters()) {
                enemyTeamComp.AddUnplayedCharacter(c);
                enemyTeamComp.RemovePlayedCharacter(c);
            }
            
            bool playerTeamRound = playerTeamFirst;
            while (playerTeamComp.GetUnplayedCharacters().Count > 0 || enemyTeamComp.GetUnplayedCharacters().Count > 0) {
                Debug.Log("Novy tah");
                chosenCharacter = null;

                currentTeam = (playerTeamRound) ? playerTeamComp : enemyTeamComp;
                playerTeamRound = !playerTeamRound;
                if (currentTeam.GetUnplayedCharacters().Count == 0)
                    continue;
                
                currentTeam.HighlightUnplayed();
                while (chosenCharacter == null) {
                    yield return new WaitForSeconds(0.5f);
                }

                chosenSkill = null;
                chosenCharacter.DisplaySkills();
                while (chosenSkill == null) {
                    yield return new WaitForSeconds(0.5f);
                }
                
                chosenTargets = new List<Character>();
                chosenSkill.HighlightTargets(playerTeamComp, enemyTeamComp);
                while (chosenSkill.GetNumOfTargets() != chosenTargets.Count) {
                    yield return new WaitForSeconds(0.5f);
                }

                chosenSkill.ApplySkill(chosenCharacter, chosenTargets);
                currentTeam.AddPlayedCharacter(chosenCharacter);
                currentTeam.RemoveUnplayedCharacter(chosenCharacter);
            }
        }

        // End battle function call
    }

    public void SetChosenCharacter(Character character) {
        chosenCharacter = character;
    }

    public Character GetChosenCharacter() {
        return chosenCharacter;
    }

    public void SetChosenSkill(Skill skill) {
        chosenSkill = skill;
    }

    public Skill GetChosenSkill() {
        return chosenSkill;
    }

    public void AddTarget(Character target) {
        chosenTargets.Add(target);
    }

    public Team GetCurrentTeam() {
        return currentTeam;
    }

    public GameObject GetSkillField() {
        return skillField;
    }
}
