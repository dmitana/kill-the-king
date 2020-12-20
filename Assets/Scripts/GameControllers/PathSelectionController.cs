using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls PathSelection scene.
/// </summary>
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

	/// <summary>
	/// Activates paths buttons based on player's current environment.
	/// </summary>
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

	/// <summary>
	/// Changes scene to selected path and increases current player's environment.
	/// Used as buttons listener.
	/// </summary>
	/// <param name="sceneName">Scene name of selected path.</param>
	public void SelectPath(String sceneName) {
		sceneController.ChangeToGameScene(sceneName);
		playerTeam.IncreaseEnvironment(Constants.SceneToEnvironmentPath[sceneName]);
	}

	/// <summary>
	/// Sets buttons non interactable.
	/// </summary>
	/// <param name="buttonsGroup">GameObject with buttons children to be set non interactable.</param>
	private void SetNonInteractable(GameObject buttonsGroup) {
		foreach (Button button in buttonsGroup.GetComponentsInChildren<Button>())
			button.interactable = false;
	}
}
