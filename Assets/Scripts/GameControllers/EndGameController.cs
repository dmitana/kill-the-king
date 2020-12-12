using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameController : MonoBehaviour {
    public TMP_Text titleText;
	public String gameOverText;
	public String gameWinText;

	private SceneController sceneController;

	void Awake() {
		titleText.text = Team.playerTeamInstance.Characters.Count > 0 ? gameWinText : gameOverText;

		sceneController = GameMaster.instance.GetComponent<SceneController>();

		// Restart static game objects
		sceneController.Restart();
		Team.playerTeamInstance.Restart();
	}

	public void BackButton() {
		sceneController.ChangeScene("MainMenu", true);
	}
}
