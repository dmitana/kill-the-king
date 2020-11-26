using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChanger : MonoBehaviour {
	private SceneController sceneController;

	void Awake() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		sceneController.ChangeFromGameScene("PathSelection", true);
	}
}
