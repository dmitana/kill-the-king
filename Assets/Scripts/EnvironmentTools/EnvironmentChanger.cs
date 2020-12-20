using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents environment border in a scene and move player to the next environment.
/// </summary>
public class EnvironmentChanger : MonoBehaviour {
	private SceneController sceneController;
	private Team playerTeam;

	void Awake() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
		playerTeam = Team.playerTeamInstance;
	}

	/// <summary>
	/// Adds skill point to player's characters and chages scene to Characters detail scene.
	/// </summary>
	/// <param name="collision">Collider that collides.</param>
	private void OnCollisionEnter2D(Collision2D collision) {
		playerTeam.AddSkillPointToCharacters();
		sceneController.ChangeFromGameScene("CharactersDetail", true);
	}
}
