using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls battle tutorial process.
/// <summary>
public class BattleTutorialController : MonoBehaviour {
	public GameObject[] instructions;
	public Button[] skillsButtons;

	private int instructionIndex = 0;

	private Team playerTeam;
	private Team enemyTeam;
	private BattleController battleController;

	private bool isPlayerCharacterSelected = false;
	private bool isListenerAddedToEnemyTeam = false;
	private bool isTargetCharacterSelected = false;
	private bool isSkillSelected = false;

	void Awake() {
		playerTeam = Team.playerTeamInstance;
		enemyTeam = GameObject.FindGameObjectWithTag("EnemyTeam").GetComponent<Team>();
		battleController = GameMaster.instance.GetComponent<BattleController>();
	}

	/// <summary>
	/// Adds listeners for events to process through tutorial's instructions.
	/// </summary>
	void OnEnable() {
		foreach (Character c in playerTeam.Characters) {
			c.onSelected += SelectPlayerCharacter;
			c.onSelected += SelectTargetCharacter;
		}

		if (battleController.battleReady) {
			foreach (Character c in enemyTeam.Characters)
				c.onSelected += SelectTargetCharacter;
			isListenerAddedToEnemyTeam = true;
		}

		foreach(Button b in skillsButtons)
			b.onClick.AddListener(SelectSkill);
	}

	/// <summary>
	/// Removes listeners from events.
	/// </summary>
	void OnDisable() {
		foreach (Character c in playerTeam.Characters) {
			c.onSelected -= SelectPlayerCharacter;
			c.onSelected -= SelectTargetCharacter;
		}

		foreach (Character c in enemyTeam.Characters)
			c.onSelected -= SelectTargetCharacter;
		isListenerAddedToEnemyTeam = false;

		foreach(Button b in skillsButtons)
			b.onClick.RemoveListener(SelectSkill);
	}

	/// <summary>
	/// Shows tutorial instructions.
	/// </summary>
    void Update() {
		if (GameMaster.IsBattleTutorialFinished) {
			Destroy(gameObject);
			return;
		}

		// Enemies don't have to be initialized when OnEnable method is executed
		if (battleController.battleReady && !isListenerAddedToEnemyTeam) {
			foreach (Character c in enemyTeam.Characters)
				c.onSelected += SelectTargetCharacter;
			isListenerAddedToEnemyTeam = true;
		}

		for (int i = 0; i < instructions.Length; i++) {
			if (i == instructionIndex)
				instructions[i].SetActive(true);
			else
				instructions[i].SetActive(false);
		}

		// Choose a character
		if (instructionIndex == 0) {
			if (isPlayerCharacterSelected) {
				++instructionIndex;
			}
		}
		// Choose a skill
		else if (instructionIndex == 1) {
			if (isSkillSelected)
				++instructionIndex;
		}
		// Choose a target
		else if (instructionIndex == 2) {
			if (isTargetCharacterSelected)
				++instructionIndex;
		}
		else {
			// Hide last instruction
			instructions[instructionIndex - 1].SetActive(false);

			GameMaster.IsBattleTutorialFinished = true;
		}
    }


	private void SelectPlayerCharacter(bool b) {
		isPlayerCharacterSelected = true;
	}

	private void SelectTargetCharacter(bool b) {
		if (isSkillSelected)
			isTargetCharacterSelected = true;
	}

	private void SelectSkill() {
		isSkillSelected = true;
	}
}
