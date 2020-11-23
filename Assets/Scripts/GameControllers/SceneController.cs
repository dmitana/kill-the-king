using System;
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
		IsGameStarted = true;
		IsGameScene = true;
		playerTeam.SetActiveCamera(true);
		playerTeam.SetActiveCharacters(true);
	}

	public Boolean ChangeFromGameScene(String newSceneName) {
		if (!IsGameScene)
			return false;

		gameSceneContainers.Push(GameObject.FindGameObjectWithTag("SceneContainer"));
		gameSceneContainers.Peek().SetActive(false);
		IsGameScene = false;
		playerTeam.SetActiveCamera(false);
		playerTeam.SetActiveCharacters(false);
		ChangeScene(newSceneName);

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
		isChangeToBattleScene = true;
		IsBattleScene = true;
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
