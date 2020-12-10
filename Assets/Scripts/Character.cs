using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Character : MonoBehaviour {
	public string characterName;
	public int maxHealth;
	public int baseStrength;
	public List<Skill> availableSkills = new List<Skill>();
	public List<Skill> skills = new List<Skill>();
	[Space]
	public HealthBarUI healthBarUI;

	public int SkillPoints { get; private set; } = 1;
	public Team Team { get; set; }

	private int health;
	private List<Effect> activeEffects = new List<Effect>();
	private BattleController battleController;
	private bool inBattle = false;

	void Awake() {
		health = maxHealth;
		
		// Creates clones of script to prevent overwriting prefab
		for (int i = 0; i < skills.Count; i++) {
			skills[i] = Instantiate(skills[i], gameObject.transform);
		}
	}

	public void AddEffect(Effect effect) {
		activeEffects.Add(effect);
	}

	public void DecreaseHealth(int damage) {
		health -= damage;
		healthBarUI.UpdateHealthBar(health, maxHealth);
		Debug.Log($"New character health: {health}");
		if (health <= 0) {
			Team.RemoveCharacterFromTeam(this);
			Destroy(gameObject);
		}
	}

	public void AddSkill(Skill skill) {
		if (!availableSkills.Contains(skill))
			throw new System.ArgumentException(
				$"Trying to add {skill.name}, which is not available for current character."
			);

		skills.Add(skill);
		--SkillPoints;
	}

	public void RemoveSkill(Skill skill) {
		skills.Remove(skill);
		++SkillPoints;
	}

	public void InitializeForBattle(BattleController controller) {
		battleController = controller;
		inBattle = true;
		healthBarUI.gameObject.SetActive(true);
		healthBarUI.UpdateHealthBar(health, maxHealth);
	}

	public void AfterBattle() {
		battleController = null;
		inBattle = false;
		healthBarUI.gameObject.SetActive(false);
		health = maxHealth;
	}

	private void OnMouseDown() {
		if (inBattle == false)
			return;

		Team currentTeam = battleController.GetCurrentTeam();
		if (battleController.ChosenCharacter == null && currentTeam.Characters.Contains(this)) {
			if (currentTeam.UnplayedCharacters.Contains(this))
				battleController.ChosenCharacter = this;
			else
				Debug.Log($"Character {characterName} was already used.");
		}
		else if (battleController.ChosenSkill != null && battleController.ValidTargets.Contains(this))
			battleController.ChosenTargets.Add(this);
	}

	public void DisplaySkills() {
		Debug.Log("Display skills");

		GameObject skillField = battleController.GetSkillField();
		UISkillField[] skillFields = skillField.transform.GetComponentsInChildren<UISkillField>();
		for (int i = 0; i < skills.Count; i++) {
			skillFields[i].SetSkill(skills[i]);
		}
	}

	public void SelectSkill() {
		Random rng = new Random();
		List<Skill> skillsNotOnCooldown = new List<Skill>();
		foreach (Skill skill in skills) {
			if (skill.cooldown == 0)
				skillsNotOnCooldown.Add(skill);
		}
		int idx = rng.Next(skillsNotOnCooldown.Count);
		battleController.ChosenSkill = skillsNotOnCooldown[idx];
	}

	public void AddSkillPoint() {
		++SkillPoints;
	}

	public void LevelUp(float healthIncPerLevel, float strengthIncPerLevel) {
		maxHealth += (int) Math.Round(maxHealth * healthIncPerLevel);
		health = maxHealth;

		baseStrength += (int) Math.Round(baseStrength * strengthIncPerLevel);
	}

	public void DecreaseCooldowns() {
		foreach (Skill skill in skills) {
			skill.DecreaseCooldown();
		}
	}
}
