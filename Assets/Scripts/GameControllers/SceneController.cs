using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public Boolean isGameStarted = false;

	private String currentSceneName = "MainMenu";
	private String currentGameSceneName;
	private GameObject currentGameSceneContainer;
	private Team playerTeam;
	private Boolean isChangeToBattleScene = false;
    private BattleController battleController;

    void Awake() {
		battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
		playerTeam = Team.playerTeamInstance;

		SceneManager.activeSceneChanged += OnChangeToBattleScene;
    }

    public void ChangeScene(String newSceneName, Boolean unloadCurrentScene = false) {
		StartCoroutine(ChangeSceneCoroutine(newSceneName, unloadCurrentScene));
    }

    private IEnumerator ChangeSceneCoroutine(String newSceneName, Boolean unloadCurrentScene) {
		Scene newScene = SceneManager.GetSceneByName(newSceneName);
		if (!newScene.IsValid()) {
			SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);
			newScene = SceneManager.GetSceneByName(newSceneName);
		}

        while (!newScene.isLoaded) {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.SetActiveScene(newScene);

		if (unloadCurrentScene) {
			AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentSceneName);
			while (!asyncUnload.isDone) {
				yield return new WaitForSeconds(0.1f);
			}
		}

		// Update current scene and game scene
		currentSceneName = newSceneName;
    }

	public void ChangeToGameScene(String newSceneName) {
		ChangeScene(newSceneName, true);
		currentGameSceneName = newSceneName;
		isGameStarted = true;
		playerTeam.SetActiveCamera(true);
		playerTeam.SetActiveCharacters(true);
	}

	public void ChangeFromGameScene(String newSceneName) {
		currentGameSceneContainer = GameObject.FindGameObjectWithTag("SceneContainer");
		currentGameSceneContainer.SetActive(false);
		playerTeam.SetActiveCamera(false);
		playerTeam.SetActiveCharacters(false);
		ChangeScene(newSceneName);
	}

	public void ResumeGameScene() {
		ChangeScene(currentGameSceneName, true);
		currentGameSceneContainer.SetActive(true);
		playerTeam.SetActiveCamera(true);
		playerTeam.SetActiveCharacters(true);
	}

	public void ChangeToBattleScene(String newSceneName, List<Character> enemies) {
		battleController.Enemies = enemies;
		isChangeToBattleScene = true;
		ChangeScene(newSceneName);
	}

	public void OnChangeToBattleScene(Scene sceneCurrent, Scene sceneNext) {
		if (isChangeToBattleScene) {
			battleController.FillEnemiesToBattle();
			battleController.InitializeCombat();
			isChangeToBattleScene = false;
		}
	}
}
