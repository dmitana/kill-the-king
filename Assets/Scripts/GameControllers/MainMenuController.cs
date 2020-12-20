using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls MainMenu scene.
/// </summary>
public class MainMenuController : MonoBehaviour {
	public Button newGameButton;
	public Button resumeGameButton;

	private SceneController sceneController;

    void OnEnable() {
		SceneManager.activeSceneChanged += EnableResumeGame;
    }

    void OnDisable() {
        SceneManager.activeSceneChanged -= EnableResumeGame;
    }

	void Start() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

	/// <summary>
	/// Callback activates Resume Game button instead of New Game button when game already started.
	/// </summary>
    private void EnableResumeGame(Scene sceneCurrent, Scene sceneNext) {
		if (sceneController != null && sceneController.IsGameStarted) {
			newGameButton.gameObject.SetActive(false);
			resumeGameButton.gameObject.SetActive(true);
		}
    }

	/// <summary>
	/// Changes scene to TeamSelection.
	/// Used as New Game button listener.
	/// </summary>
	public void NewGameButton() {
		sceneController.ChangeScene("TeamSelection", true);
	}

	/// <summary>
	/// Resumes game scene.
	/// Used as Resume Game button listener.
	/// </summary>
	public void ResumeGameButton() {
		sceneController.ResumeGameScene();
	}

	/// <summary>
	/// Quits application.
	/// Used as Quit button listener.
	/// </summary>
	public void QuitGameButton() {
		Application.Quit();
	}
}
