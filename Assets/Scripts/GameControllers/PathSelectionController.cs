using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSelectionController : MonoBehaviour {
	private SceneController sceneController;

	void Awake() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

	public void ForestRoadButton() {
		sceneController.ChangeToGameScene("ForestRoad");
	}
}
