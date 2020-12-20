using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls TeamSelection scene.
/// </summary>
public class TeamSelectionController : MonoBehaviour {
	// UI game objects
	public GameObject assassinUI;
	public GameObject darkKnightUI;
	public GameObject darkWizardUI;
	[Space]
	public CharacterDetailUI characterDetailUI;
	public GameObject teamSelectionUI;
	public Button createTeamButton;
	[Space]
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

		// Add listeners for Assassin UI
		assassinToggle = assassinUI.GetComponentInChildren<Toggle>();
		assassinButton = assassinUI.GetComponentInChildren<Button>();
		assassinToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, assassin));
		assassinButton.onClick.AddListener(() => CreateCharacterDetailUI(assassin));

		// Add listeners for Dark Knight UI
		darkKnightToggle = darkKnightUI.GetComponentInChildren<Toggle>();
		darkKnightButton = darkKnightUI.GetComponentInChildren<Button>();
		darkKnightToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, darkKnight));
		darkKnightButton.onClick.AddListener(() => CreateCharacterDetailUI(darkKnight));

		// Add listeners for Dark Knight UI
		darkWizardToggle = darkWizardUI.GetComponentInChildren<Toggle>();
		darkWizardButton = darkWizardUI.GetComponentInChildren<Button>();
		darkWizardToggle.onValueChanged.AddListener((change) => AddRemoveCharacter(change, darkWizard));
		darkWizardButton.onClick.AddListener(() => CreateCharacterDetailUI(darkWizard));
	}

	void Update() {
		CanSelectCharacter(assassin, assassinToggle);
		CanSelectCharacter(darkKnight, darkKnightToggle);
		CanSelectCharacter(darkWizard, darkWizardToggle);
		CanCreateTeam();
	}

	/// <summary>
	/// Controls whether character can be selected to the team.
	/// Character can be selected only if its skills are selected.
	/// </summary>
	/// <param name="character">Character to be controlled</param>
	/// <param name="characterToggle">Character's toogle to be set (not) interactable.</param>
	private void CanSelectCharacter(Character character, Toggle characterToggle) {
		if (character.SkillPoints == 0) {
			characterToggle.interactable = true;
		}
		else {
			characterToggle.interactable = false;
			characterToggle.isOn = false;
		}
	}

	/// <summary>
	/// Controls whether team can be created.
	/// Team can be created when 3 characters are selected.
	/// </summary>
	private void CanCreateTeam() {
		createTeamButton.interactable = playerTeam.Characters.Count == 3 ? true : false;
	}

	/// <summary>
	/// Created character detail UI for selected character.
	/// If UI was already created, activate it instead.
	/// </summary>
	/// <param name="character">Character to be detail UI created/activated for.</param>
	private void CreateCharacterDetailUI(Character character) {
		currentCharacterDetailUI?.gameObject.SetActive(false);

		if (characterDetailUIMapping.ContainsKey(character.characterName)) {
			currentCharacterDetailUI = characterDetailUIMapping[character.characterName];
			currentCharacterDetailUI.gameObject.SetActive(true);
		}
		else {
			currentCharacterDetailUI = Instantiate(characterDetailUI, teamSelectionUI.transform);
			currentCharacterDetailUI.Show(character);
			characterDetailUIMapping.Add(character.characterName, currentCharacterDetailUI);
		}
	}

	/// <summary>
	/// Adds or removes character to/from player team based on toggle change.
	/// </summary>
	/// <param name="change">Change of character toggle.</param>
	/// <param name="character">Character to be added or removed to/from team.</param>
	private void AddRemoveCharacter(bool change, Character character) {
		if (change)
			playerTeam.AddCharacterToTeam(character);
		else
			playerTeam.RemoveCharacterFromTeam(character);
	}

	/// <summary>
	/// Sets parent for each character in the team and changes scene to Path selection.
	/// Used as Create Team button listener.
	/// </summary>
	public void CreateTeamButton() {
		playerTeam.SetCharactersParent();
		sceneController.ChangeScene("PathSelection", true);
	}

	/// <summary>
	/// Removes all characters from the team and change scene to Main menu.
	/// Used as Back button listener.
	/// </summary>
	public void BackButton() {
		playerTeam.ClearCharacters();
		sceneController.ChangeScene("MainMenu", true);
	}
}
