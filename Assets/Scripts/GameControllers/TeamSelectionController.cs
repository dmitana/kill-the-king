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
	public CharacterDetailUI characterDetailUI;
	public GameObject teamSelectionUI;

	// Public instantiated prefabs
	public Character assassin;
	public Character darkKnight;

	// Assassin's UI elements
	private Toggle assassinToggle;
	private Button assassinButton;

	// Dark Knight's UI elements
	private Toggle darkKnightToggle;
	private Button darkKnightButton;

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
	}

	void Update() {
		CanSelectCharacter(assassin, assassinToggle);
		CanSelectCharacter(darkKnight, darkKnightToggle);
	}

	private void CanSelectCharacter(Character character, Toggle characterToggle) {
		characterToggle.interactable = character.SkillPoints == 0 ? true : false;
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
		if (change) {
			playerTeam.AddCharacterToTeam(character);
			character.SetTeam(playerTeam);
		}
		else {
			playerTeam.RemoveCharacterFromTeam(character);
			character.SetTeam(null);
		}
	}

	public void CreateTeamButton() {
		playerTeam.SetCharactersParent();
		sceneController.ChangeScene("PathSelection", true);
	}

	public void BackButton() {
		sceneController.ChangeScene("MainMenu", true);
	}
}
