﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour {
	public List<GameObject> charactersUI;

	private Team playerTeam;
	private SceneController sceneController;

	void Awake() {
		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

	void Start() {
		Show();
	}

	private void Show() {
		for (int i = 0; i < charactersUI.Count; ++i) {
			if (i >= playerTeam.Characters.Count) {
				charactersUI[i].gameObject.SetActive(false);
				continue;
			}
			var characterTexts = charactersUI[i].GetComponentsInChildren<TMP_Text>();
			characterTexts[0].text = playerTeam.Characters[i].characterName;
			characterTexts[1].text = $"Health: {playerTeam.Characters[i].maxHealth}";
			characterTexts[2].text = $"Strength: {playerTeam.Characters[i].baseStrength}";
		}
	}

	public void OnClick() {
		sceneController.ChangeFromGameScene("CharactersDetail");
	}
}