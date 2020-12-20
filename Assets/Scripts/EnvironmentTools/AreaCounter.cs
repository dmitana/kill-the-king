using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents area border in a scene and increments player's area.
/// </summary>
public class AreaCounter : MonoBehaviour {
	private Team playerTeam;

	void Awake() {
		playerTeam = Team.playerTeamInstance;
	}

	/// <summary>
	/// Increases player's area number when triggered by the player team.
	/// </summary>
	/// <param name="other">Collider that triggers.</param>
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<Character>()?.Team != playerTeam)
			return;
		playerTeam.IncreaseArea();
		Destroy(gameObject);
	}

}
