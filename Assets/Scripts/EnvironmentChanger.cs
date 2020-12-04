using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChanger : MonoBehaviour {
	private SceneController sceneController;
	private Team playerTeam;

	void Awake() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
		playerTeam = Team.playerTeamInstance;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		playerTeam.AddSkillPointToCharacters();
		sceneController.ChangeFromGameScene("CharactersDetail", true);
	}
}
