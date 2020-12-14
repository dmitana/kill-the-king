using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCounter : MonoBehaviour {
	private Team playerTeam;

	void Awake() {
		playerTeam = Team.playerTeamInstance;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<Character>()?.Team != playerTeam)
			return;
		playerTeam.IncreaseArea();
		Destroy(gameObject);
	}

}
