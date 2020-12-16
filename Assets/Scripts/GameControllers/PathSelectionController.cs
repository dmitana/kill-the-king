using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathSelectionController : MonoBehaviour {
	public GameObject countrySideEnvButtons;
	public GameObject townEnvButtons;
	public GameObject castleEnvButtons;
	public GameObject royalHallButton;

	private SceneController sceneController;
	private Team playerTeam;

	void Awake() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
		playerTeam = Team.playerTeamInstance;
	}

	void Start() {
		if (playerTeam.CurrentEnvironment > 0) {
			townEnvButtons.SetActive(true);
			SetNonInteractable(countrySideEnvButtons);
		}
		if (playerTeam.CurrentEnvironment > 1) {
			castleEnvButtons.SetActive(true);
			SetNonInteractable(townEnvButtons);
		}
		if (playerTeam.CurrentEnvironment > 2) {
			royalHallButton.SetActive(true);
			SetNonInteractable(castleEnvButtons);
		}
	}

	public void SelectPath(String sceneName) {
		sceneController.ChangeToGameScene(sceneName);
		playerTeam.IncreaseEnvironment(Constants.SceneToEnvironmentPath[sceneName]);
	}

	private void SetNonInteractable(GameObject buttonsGroup) {
		foreach (Button button in buttonsGroup.GetComponentsInChildren<Button>())
			button.interactable = false;
	}
}
