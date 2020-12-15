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
	public GameObject darkWizardUI;
	public CharacterDetailUI characterDetailUI;
	public GameObject teamSelectionUI;

	// Public instantiated prefabs
	public Character assassin;
	public Character darkKnight;
	public Character darkWizard;

	// Assassin's UI elements
	private Toggle assassinToggle;
	private Button assassinButton;

	// Dark Knight's UI elements
	private Toggle darkKnightToggle;
	private Button darkKnightButton;
	
	// Dark Wizard's UI elements
	private Toggle darkWizardToggle;
	private Button darkWizardButton;

	private CharacterDetailUI currentCharacterDetailUI;
	private Dictionary<String, CharacterDetailUI> characterDetailUIMapping = new Dictionary<String, CharacterDetailUI>();

	private Team playerTeam;
	private SceneController sceneController;

	void Awake() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
		playerTeam = Team.playerTeamInstance;

		// Add listerens for Assassin UI
		assassinToggle = assassinUI.GetComponentInChildren<Toggle>();
		assassinButton = assassinUI.GetComponentInChildren<Button>();
		assassinToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, assassin));
		assassinButton.onClick.AddListener(() => CreateCharacterDetailUI(assassin));

		// Add listerens for Dark Knight UI
		darkKnightToggle = darkKnightUI.GetComponentInChildren<Toggle>();
		darkKnightButton = darkKnightUI.GetComponentInChildren<Button>();
		darkKnightToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, darkKnight));
		darkKnightButton.onClick.AddListener(() => CreateCharacterDetailUI(darkKnight));
		
		// Add listerens for Dark Knight UI
		darkWizardToggle = darkWizardUI.GetComponentInChildren<Toggle>();
		darkWizardButton = darkWizardUI.GetComponentInChildren<Button>();
		darkWizardToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, darkWizard));
		darkWizardButton.onClick.AddListener(() => CreateCharacterDetailUI(darkWizard));
	}

	void Update() {
		CanSelectCharacter(assassin, assassinToggle);
		CanSelectCharacter(darkKnight, darkKnightToggle);
		CanSelectCharacter(darkWizard, darkWizardToggle);
	}

	private void CanSelectCharacter(Character character, Toggle characterToggle) {
		if (character.SkillPoints == 0) {
			characterToggle.interactable = true;
		}
		else {
			characterToggle.interactable = false;
			characterToggle.isOn = false;
		}
	}

	private void CreateCharacterDetailUI(Character character) {
		currentCharacterDetailUI?.gameObject.SetActive(false);

		if (characterDetailUIMapping.ContainsKey(character.characterName)) {
			Debug.Log(characterDetailUIMapping[character.characterName]);
			currentCharacterDetailUI = characterDetailUIMapping[character.characterName];
			currentCharacterDetailUI.gameObject.SetActive(true);
		}
		else {
			currentCharacterDetailUI = Instantiate(characterDetailUI, teamSelectionUI.transform);
			currentCharacterDetailUI.Show(character);
			characterDetailUIMapping.Add(character.characterName, currentCharacterDetailUI);
		}
	}

	private void AddRemoveCharacter(bool change, Character character) {
		if (change)
			playerTeam.AddCharacterToTeam(character);
		else
			playerTeam.RemoveCharacterFromTeam(character);
	}

	public void CreateTeamButton() {
		playerTeam.SetCharactersParent();
		sceneController.ChangeScene("PathSelection", true);
	}

	public void BackButton() {
		playerTeam.ClearCharacters();
		sceneController.ChangeScene("MainMenu", true);
	}
}
