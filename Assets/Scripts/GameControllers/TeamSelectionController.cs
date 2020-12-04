using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamSelectionController : MonoBehaviour {
	// UI game objects
	public GameObject assassinUI;
	public GameObject darkKnightUI;
	public GameObject characterDetailUI;
	public List<GameObject> skillsUI;

	// Public instantiated prefabs
	public Character assassin;
	public Character darkKnight;

	// Assassin's UI elements
	private Toggle assassinToggle;
	private Button assassinButton;
	
	// Dark Knight's UI elements
	private Toggle darkKnightToggle;
	private Button darkKnightButton;

	// Character detail UI childen
	private TMP_Text characterNameText;
	private TMP_Text characterHealthText;
	private TMP_Text characterStrenghtText;

	// Private Instantiated prefabs
	private Team playerTeam;

	private SceneController sceneController;

	void Awake() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
		playerTeam = Team.playerTeamInstance;

		// Add listerens for Assassin UI
		assassinToggle = assassinUI.GetComponentInChildren<Toggle>();
		assassinButton = assassinUI.GetComponentInChildren<Button>();
		assassinToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, assassin));
		assassinButton.onClick.AddListener(() => ShowCharacter(assassin));
		
		// Add listerens for Dark Knight UI
		darkKnightToggle = darkKnightUI.GetComponentInChildren<Toggle>();
		darkKnightButton = darkKnightUI.GetComponentInChildren<Button>();
		darkKnightToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, darkKnight));
		darkKnightButton.onClick.AddListener(() => ShowCharacter(darkKnight));

		// Get character detail UI childen
		var texts = characterDetailUI.GetComponentsInChildren<TMP_Text>();
		characterNameText = texts[0];
		characterHealthText = texts[1];
		characterStrenghtText = texts[2];
	}

	void Update() {
		CanSelectCharacter(assassin, assassinToggle);
		CanSelectCharacter(darkKnight, darkKnightToggle);
	}

	private void CanSelectCharacter(Character character, Toggle characterToggle) {
		characterToggle.interactable = character.skills.Count == 2 ? true : false;
	}

	private void ShowCharacter(Character character) {
		characterDetailUI.SetActive(true);
		characterNameText.text = character.characterName;
		characterHealthText.text = $"Health: {character.health}";
		characterStrenghtText.text = $"Strength: {character.baseStrength}";

		// Create UI for each skill
		if (character.availableSkills.Count != skillsUI.Count)
			throw new System.ArgumentException(
				$"Character must have {skillsUI.Count} skill, but has {character.availableSkills.Count}"
			);

		for (int i = 0;  i < skillsUI.Count; ++i) {
			var skillTexts = skillsUI[i].GetComponentsInChildren<TMP_Text>();
			skillTexts[0].text = character.availableSkills[i].skillName;
			skillTexts[1].text = character.availableSkills[i].description;

			int n = i;
			Toggle toggle = skillsUI[i].GetComponentInChildren<Toggle>();
			toggle.onValueChanged.AddListener(
				(change) => AddRemoveSkill(change, character.availableSkills[n], character)
			);
		}
	}

	private void AddRemoveCharacter(bool change, Character character) {
		if (change) {
			playerTeam.AddCharacterToTeam(character);
			character.SetTeam(playerTeam);
		}
		else {
			playerTeam.RemoveCharacterFromTeam(character);
			character.SetTeam(null);
		}
	}

	private void AddRemoveSkill(bool change, Skill skill, Character character) {
		if (change)
			character.AddSkill(skill);
		else
			character.RemoveSkill(skill);
	}

	public void CreateTeamButton() {
		playerTeam.SetCharactersParent();
		sceneController.ChangeScene("PathSelection", true);
	}

	public void BackButton() {
		sceneController.ChangeScene("MainMenu", true);
	}
}
