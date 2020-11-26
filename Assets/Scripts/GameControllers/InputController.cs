﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour {
	private SceneController sceneController;

	void Start() {
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

    void Update() {
		if (Input.GetButtonDown("Cancel")) {
			sceneController.ChangeFromGameScene("MainMenu");
		}
    }
}