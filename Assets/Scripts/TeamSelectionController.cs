using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamSelectionController : MonoBehaviour {
	// UI game objects
	public GameObject assassinUI;
	public GameObject characterDetailUI;
	public List<GameObject> skillsUI;

	// Prefabs
	public Team team;
	public Character assassin;

	// Assassin's UI elements
	private Toggle assassinToggle;
	private Button assassinButton;

	// Character detail UI childen
	private TMP_Text characterNameText;
	private TMP_Text characterHealthText;
	private TMP_Text characterStrenghtText;

	// Instantiated prefabs
	private Team teamInstantiated;
	private Character assassinInstantiated;

	void Awake() {
		// Instantiate prefabs
		teamInstantiated = Instantiate(team);
		assassinInstantiated = Instantiate(assassin);
		assassinInstantiated.gameObject.SetActive(false);

		// Add listerens for Assassin UI
		assassinToggle = assassinUI.GetComponentInChildren<Toggle>();
		assassinButton = assassinUI.GetComponentInChildren<Button>();
		assassinToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, assassinInstantiated));
		assassinButton.onClick.AddListener(() => ShowCharacter(assassinInstantiated));

		// Get character detail UI childen
		var texts = characterDetailUI.GetComponentsInChildren<TMP_Text>();
		characterNameText = texts[0];
		characterHealthText = texts[1];
		characterStrenghtText = texts[2];
	}

	void Update() {
		CanSelectCharacter(assassinInstantiated, assassinToggle);
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
		if (change)
			teamInstantiated.AddCharacterToTeam(character);
		else
			teamInstantiated.RemoveCharacterFromTeam(character);
	}

	private void AddRemoveSkill(bool change, Skill skill, Character character) {
		if (change)
			character.AddSkill(skill);
		else
			character.RemoveSkill(skill);
	}

	public void CreateTeamButton() {
		teamInstantiated.SetCharactersParent();
		SceneManager.LoadScene("PathSelection");
	}

}
