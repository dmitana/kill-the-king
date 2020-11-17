﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
	public static GameMaster instance;

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(instance);
		}
		else
			Destroy(gameObject);
	}
}
