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
    private Skill chosenSkill;

    private bool playerTeamFirst;

    private Random rng;

    private void Awake() {
        rng = new Random();
    }

    public void FillEnemiesToBattle() {
        enemyTeam = GameObject.FindGameObjectWithTag("EnemyTeam");
		Transform enemyTeamTransform = enemyTeam.transform;
		Vector3 position = enemyTeamTransform.position;

		foreach (Character enemy in Enemies) {
			Instantiate(enemy, position, Quaternion.identity, enemyTeamTransform);
			position[0] += 1;
		}
    }

    public void InitializeCombat() {
        playerTeam = GameObject.FindGameObjectWithTag("PlayerTeam");
        playerTeam.transform.position = new Vector3(-5, 0, 0);
        playerTeam.GetComponent<CharacterMovement>().enabled = false;

        playerTeamFirst = (rng.Next(2) == 0);
    }

    private IEnumerator Combat() {
        Team playerTeamComp = (Team) playerTeam.GetComponent(typeof(Team));
        Team enemyTeamComp = (Team) enemyTeam.GetComponent(typeof(Team));

        Team nextTeam;

        while (playerTeamComp.GetCharacters().Count > 0 && enemyTeamComp.GetCharacters().Count > 0) {

            // set unplayed characters - each character will have some status attribute telling whether it can be clicked
            bool playerTeamRound = playerTeamFirst;
            while (playerTeamComp.GetUnplayedCharacters().Count > 0 || enemyTeamComp.GetUnplayedCharacters().Count > 0) {
                chosenCharacter = null;

                nextTeam = (playerTeamRound) ? playerTeamComp : enemyTeamComp;
                nextTeam.HighlightUnplayed();
                while (chosenCharacter == null) {
                    yield return new WaitForSeconds(0.5f);
                }

                chosenSkill = null;
                chosenCharacter.DisplaySkills();
                while (chosenSkill == null) {
                    yield return new WaitForSeconds(0.5f);
                }

                // pick character from team
                // pick skill
                // attack
                // move used character to PlayedCharacters list
                playerTeamRound = !playerTeamRound;
            }

        }

        // End battle function call
    }

    public void SetChosenCharacter(Character character) {
        chosenCharacter = character;
    }

    public void SetChosenSkill(Skill skill) {
        chosenSkill = skill;
    }
}
