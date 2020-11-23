using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void EnableResumeGame(Scene sceneCurrent, Scene sceneNext) {
		if (sceneController != null && sceneController.IsGameStarted) {
			newGameButton.gameObject.SetActive(false);
			resumeGameButton.gameObject.SetActive(true);
		}
    }

	public void NewGameButton() {
		sceneController.ChangeScene("TeamSelection", true);
	}

	public void ResumeGameButton() {
		sceneController.ResumeGameScene();
	}

	public void QuitGameButton() {
		Application.Quit();
	}
}
