using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathSelectionController : MonoBehaviour {
	private Team team;

	void Awake() {
		team = GameObject.FindGameObjectWithTag("PlayerTeam").GetComponent<Team>();
	}

	public void ForestRoadButton() {
		SceneManager.LoadScene("ForestRoad");
		team.EnableCamera();
		team.EnableCharacters();
	}
}
