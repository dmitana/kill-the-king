using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls player's input.
/// </summary>
public class InputController : MonoBehaviour {
	private SceneController sceneController;

	public delegate void OnInputDelegate();
	public event OnInputDelegate onCharactersOpen;
	public event OnInputDelegate onMapOpen;
	public event OnInputDelegate onMainMenuOpen;

	void Start() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

	/// <summary>
	/// Performs actions based on player's input.
	/// </summary>
    void Update() {
		if (Input.GetButtonDown("Cancel")) {
			onMainMenuOpen?.Invoke();
			sceneController.ChangeFromGameScene("MainMenu");
		}

		if (Input.GetButtonDown("Map")) {
			onMapOpen?.Invoke();
			sceneController.ChangeFromGameScene("Map");
		}

		if (Input.GetButtonDown("Characters")) {
			onCharactersOpen?.Invoke();
			sceneController.ChangeFromGameScene("CharactersDetail");
		}
    }
}
