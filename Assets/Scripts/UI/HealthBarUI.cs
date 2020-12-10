using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
	public Character character;
	[Space]
	public Slider healthBar;
	public TMP_Text healthText;

	void Update() {
		if (character.InBattle) {
			healthBar.gameObject.SetActive(true);
			healthBar.value = (float) character.Health / character.maxHealth;
			healthText.text = $"{character.Health}/{character.maxHealth}";
		}
		else
			healthBar.gameObject.SetActive(false);
	}
}
