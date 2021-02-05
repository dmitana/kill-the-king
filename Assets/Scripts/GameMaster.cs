using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents Game Master game object which is singleton.
/// Game Master game object has components such as BattleController and SceneController.
/// </summary>
public class GameMaster : MonoBehaviour {
	public static GameMaster instance;
	public static bool IsBattleTutorialFinished { get; set; } = false;

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(instance);
		}
		else
			Destroy(gameObject);
	}
}
