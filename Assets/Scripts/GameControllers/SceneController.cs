﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public Boolean IsGameStarted { get; private set; } = false;
	public Boolean IsGameScene { get; private set; } = false;
	public Boolean IsBattleScene { get; private set; } = false;

	private String currentSceneName = "MainMenu";
	private String currentGameSceneName;
	private Stack<GameObject> gameSceneContainers = new Stack<GameObject>();
	private Team playerTeam;
	private Boolean isChangeToBattleScene = false;
	private Boolean isChangeToGameScene = false;
    private BattleController battleController;

    void Awake() {
		battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
		playerTeam = Team.playerTeamInstance;

		SceneManager.activeSceneChanged += OnChangeToGameScene;
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
		IsGameStarted = true;
		IsGameScene = true;

		// Activate change scene callback
		isChangeToGameScene = true;
	}

	public void OnChangeToGameScene(Scene sceneCurrent, Scene sceneNext) {
		if (!isChangeToGameScene)
			return;

		// Place player team to initial position
		GameObject playerTeamInitialPosition = GameObject.FindGameObjectWithTag("PlayerTeamInitialPosition");
		playerTeam.gameObject.transform.position = playerTeamInitialPosition.transform.position;

		// Activate player team
		playerTeam.SetActiveCamera(true);
		playerTeam.SetActiveCharacters(true);

		// Deactivate change scene callback
		isChangeToGameScene = false;
	}

	public Boolean ChangeFromGameScene(String newSceneName, Boolean unloadCurrentScene = false) {
		if (!IsGameScene)
			return false;

		if (!unloadCurrentScene) {
			gameSceneContainers.Push(GameObject.FindGameObjectWithTag("SceneContainer"));
			gameSceneContainers.Peek().SetActive(false);
		}
		else
			IsGameStarted = false;

		IsGameScene = false;
		playerTeam.SetActiveCamera(false);
		playerTeam.SetActiveCharacters(false);
		ChangeScene(newSceneName, unloadCurrentScene);

		return true;
	}

	public Boolean ResumeGameScene() {
		if (IsGameScene)
			return false;

		ChangeScene(currentGameSceneName, true);
		gameSceneContainers.Pop().SetActive(true);
		IsGameScene = true;
		if (!IsBattleScene)
			playerTeam.SetActiveCamera(true);
		playerTeam.SetActiveCharacters(true);

		return true;
	}

	public void ChangeToBattleScene(String newSceneName, List<Character> enemies) {
		gameSceneContainers.Push(GameObject.FindGameObjectWithTag("SceneContainer"));
		gameSceneContainers.Peek().SetActive(false);
		playerTeam.SetActiveCamera(false);
		battleController.Enemies = enemies;

		ChangeScene(newSceneName);
		IsBattleScene = true;

		// Activate change scene callback
		isChangeToBattleScene = true;
	}

	public void OnChangeToBattleScene(Scene sceneCurrent, Scene sceneNext) {
		if (!isChangeToBattleScene)
			return;

		battleController.FillEnemiesToBattle();
		battleController.InitializeCombat();

		// Deactivate change scene helper
		isChangeToBattleScene = false;
	}
}
