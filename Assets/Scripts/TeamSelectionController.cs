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
	public GameObject skillUI;

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

	void Awake() {
		// Add listerens for Assassin UI
		assassinToggle = assassinUI.GetComponentInChildren<Toggle>();
		assassinButton = assassinUI.GetComponentInChildren<Button>();
		assassinToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, assassin));
		assassinButton.onClick.AddListener(() => ShowCharacter(assassin));

		// Get character detail UI childen
		var texts = characterDetailUI.GetComponentsInChildren<TMP_Text>();
		characterNameText = texts[0];
		characterHealthText = texts[1];
		characterStrenghtText = texts[2];

		// Instantiate team
		teamInstantiated = Instantiate(team);
	}

	private void ShowCharacter(Character c) {
		characterDetailUI.SetActive(true);
		characterNameText.text = c.name;
		characterHealthText.text = $"Health: {c.health}";
		characterStrenghtText.text = $"Strength: {c.baseStrength}";

		// Create UI for each skill
		foreach (Skill skill in c.skills) {
			var skillUIInstantiated = Instantiate(skillUI, characterDetailUI.transform, false);
			var skillTexts = skillUIInstantiated.GetComponentsInChildren<TMP_Text>();
			skillTexts[0].text = skill.skillName;
			skillTexts[1].text = skill.description;
		}
	}

	private void AddRemoveCharacter(bool change, Character c) {
		if (change)
			teamInstantiated.AddCharacterToTeam(c);
		else
			teamInstantiated.RemoveCharacterFromTeam(c);
	}

	public void CreateTeamButton() {
		teamInstantiated.InstantiateCharacters();
		SceneManager.LoadScene("ForestRoad");
		teamInstantiated.EnableCamera();
		teamInstantiated.EnableCharacters();
	}

}
