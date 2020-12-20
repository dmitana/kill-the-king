using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Represents scene controller which controls scene changing.
/// </summary>
public class SceneController : MonoBehaviour {
	public Boolean IsGameStarted { get; private set; } = false;
	public Boolean IsGameScene { get; private set; } = false;
	public Boolean IsBattleScene { get; private set; } = false;

	private String currentSceneName = "MainMenu";
	private String currentGameSceneName;
	private String currentBattleSceneName;
	/// <summary>
	/// Stack of game scene containers used for returning to previous scenes.
	/// </summary>
	private Stack<GameObject> gameSceneContainers = new Stack<GameObject>();
	private Team playerTeam;
	private Boolean isChangeToBattleScene = false;
	private Boolean isChangeToGameScene = false;
	private Boolean isReturnFromBattleScene = false;
	private Action returnFromBattleCallback;
    private BattleController battleController;

    void Awake() {
		battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();

		SceneManager.activeSceneChanged += OnChangeToGameScene;
		SceneManager.activeSceneChanged += OnChangeToBattleScene;
		SceneManager.activeSceneChanged += OnReturnFromBattleScene;
    }

	void Start() {
		playerTeam = Team.playerTeamInstance;
	}

	/// <summary>
	/// Restarts scene controller to default state.
	/// </summary>
	public void Restart() {
		IsGameStarted = false;
		IsGameScene = false;
		IsBattleScene = false;
		gameSceneContainers = new Stack<GameObject>();
	}

	/// <summary>
	/// Starts coroutine to change scene.
	/// </summary>
	/// <param name="newSceneName">Name of scene to which it will be changed.</param>
	/// <param name="unloadCurrentScene">Whether to unload current scene or not.</param>
    public void ChangeScene(String newSceneName, Boolean unloadCurrentScene = false) {
		StartCoroutine(ChangeSceneCoroutine(newSceneName, unloadCurrentScene));
    }

	/// <summary>
	/// Coroutine to change scene.
	/// </summary>
	/// <param name="newSceneName">Name of scene to which it will be changed.</param>
	/// <param name="unloadCurrentScene">Whether to unload current scene or not.</param>
    private IEnumerator ChangeSceneCoroutine(String newSceneName, Boolean unloadCurrentScene) {
		Scene newScene = SceneManager.GetSceneByName(newSceneName);
		if (!newScene.IsValid()) {
			SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);
			newScene = SceneManager.GetSceneByName(newSceneName);
		}

        while (!newScene.isLoaded) {
            yield return new WaitForSeconds(0.1f);
        }

		// Update current scene name
		string prevSceneName = currentSceneName;
		currentSceneName = newSceneName;
        SceneManager.SetActiveScene(newScene);

		if (unloadCurrentScene) {
			AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(prevSceneName);
			while (!asyncUnload.isDone) {
				yield return new WaitForSeconds(0.1f);
			}
		}

    }

	/// <summary>
	/// Sets appropriate values and changes scene to game scene.
	/// </summary>
	/// <param name="newSceneName">Name of scene to which it will be changed.</param>
	public void ChangeToGameScene(String newSceneName) {
		currentGameSceneName = newSceneName;
		IsGameStarted = true;
		IsGameScene = true;

		// Activate change scene callback
		isChangeToGameScene = true;
		ChangeScene(newSceneName, true);
	}

	/// <summary>
	/// Callback called after scene is changed to game scene.
	/// Performs necessary actions after change to game scene.
	/// </summary>
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

	/// <summary>
	/// Sets appropriate values and change from game scene.
	/// </summary>
	/// <param name="newSceneName">Name of scene to which it will be changed.</param>
	/// <param name="unloadCurrentScene">Whether to unload current scene or not.</param>
	/// <returns>`true` if scene was changed otherwise `false`.</returns>
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

	/// <summary>
	/// Resumes last game scene.
	/// </summary>
	/// <returns>`true` if scene was changed otherwise `false`.</returns>
	public Boolean ResumeGameScene() {
		if (IsGameScene)
			return false;

		var sceneName = currentGameSceneName;

		playerTeam.SetActiveCharacters(true);
		gameSceneContainers.Pop().SetActive(true);
		IsGameScene = true;
		if (IsBattleScene)
			sceneName = currentBattleSceneName;
		else
			playerTeam.SetActiveCamera(true);

		ChangeScene(sceneName, true);

		return true;
	}

	/// <summary>
	/// Sets appropriate values and changes scene to battle scene.
	/// </summary>
	/// <param name="newSceneName">Name of scene to which it will be changed.</param>
	/// <param name="enemies">Enemies to be filled to the battle.</param>
	/// <param name="returnFromBattleCallback">Callback to be called on return from battle.</param>
	public void ChangeToBattleScene(String newSceneName, List<Character> enemies,
			Action returnFromBattleCallback) {
		// Activate change scene callback
		isChangeToBattleScene = true;

		gameSceneContainers.Push(GameObject.FindGameObjectWithTag("SceneContainer"));
		gameSceneContainers.Peek().SetActive(false);
		playerTeam.SetActiveCamera(false);
		battleController.Enemies = enemies;
		currentBattleSceneName = newSceneName;

		IsBattleScene = true;
		this.returnFromBattleCallback = returnFromBattleCallback;
		ChangeScene(newSceneName);
	}

	/// <summary>
	/// Callback called after scene is changed to battle scene.
	/// Initializes battle when battle scene is ready.
	/// </summary>
	public void OnChangeToBattleScene(Scene sceneCurrent, Scene sceneNext) {
		if (!isChangeToBattleScene)
			return;

		battleController.InitializeBattle();

		// Deactivate change scene callback
		isChangeToBattleScene = false;
	}

	/// <summary>
	/// Sets appropriate values and returns from battle scene to previous game scene.
	/// </summary>
	/// <returns>`true` if scene was changed otherwise `false`.</returns>
	public Boolean ReturnFromBattleScene() {
		if (!IsBattleScene)
			return false;

		// Activate change scene callback
		isReturnFromBattleScene = true;

		IsBattleScene = false;
		playerTeam.SetActiveCamera(true);
		gameSceneContainers.Pop().SetActive(true);
		ChangeScene(currentGameSceneName, true);

		return true;
	}

	/// <summary>
	/// Callback called after return from battle scene.
	/// Calls callback set in <c>ChangeToBattleScene</c>.
	/// </summary>
	public void OnReturnFromBattleScene(Scene sceneCurrent, Scene sceneNext) {
		if (!isReturnFromBattleScene)
			return;

		returnFromBattleCallback();

		// Deactivate change scene callback
		isReturnFromBattleScene = false;
	}

	/// <summary>
	/// Changes scene after loosing a game.
	/// </summary>
	public void EndGameLoss() {
		AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentGameSceneName);
		ChangeScene("EndGame", true);
	}

	/// <summary>
	/// Changes scene after winning a game.
	/// </summary>
	public void EndGameWin() {
		playerTeam.SetActiveCamera(false);
		playerTeam.SetActiveCharacters(false);
		ChangeScene("EndGame", true);
	}
}
