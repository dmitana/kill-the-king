using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
	public Slider healthBar;
	public TMP_Text healthText;

	public void UpdateHealthBar(int health, int maxHealth) {
		healthBar.value = (float) health / maxHealth;
		healthText.text = $"{health}/{maxHealth}";
	}
}
