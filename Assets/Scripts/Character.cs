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

	public int SkillPoints { get; private set; } = 1;
	public Team Team { get; set; }
	public int Health { get; private set; }
	public bool InBattle { get; private set; } = false;

	private List<Effect> activeEffects = new List<Effect>();
	private BattleController battleController;

	void Awake() {
		Health = maxHealth;

		// Creates clones of script to prevent overwriting prefab
		for (int i = 0; i < skills.Count; i++) {
			skills[i] = Instantiate(skills[i], gameObject.transform);
			for (int j = 0; j < skills[i].effects.Count; j++) {
				skills[i].effects[j] = Instantiate(skills[i].effects[j], skills[i].gameObject.transform);
			}
		}
	}

	public void AddEffect(Effect effect) {
		activeEffects.Add(effect);
	}

	public void ApplyEffects() {
		int i = 0;
		while (i < activeEffects.Count) {
			if (activeEffects[i].duration == 0) {
				activeEffects.RemoveAt(i);
				continue;
			}
			activeEffects[i].applyEffect(this);
			i++;
		}
	}

	public void DecreaseHealth(int damage) {
		Health -= damage;
		if (Health <= 0) {
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
		InBattle = true;
	}

	public void AfterBattle() {
		battleController = null;
		InBattle = false;
		Health = maxHealth;
		foreach (Skill skill in skills) {
			skill.cooldown = 0;
		}
	}

	private void OnMouseDown() {
		if (InBattle == false)
			return;

		Team currentTeam = battleController.GetCurrentTeam();
		if (battleController.ChosenCharacter == null && currentTeam.Characters.Contains(this)) {
			if (currentTeam.UnplayedCharacters.Contains(this))
				battleController.ChosenCharacter = this;
			else
				battleController.Log = $"Character {characterName} was already used.";
		}
		else if (battleController.ChosenSkill != null && battleController.ValidTargets.Contains(this))
			battleController.ChosenTargets.Add(this);
	}

	public void DisplaySkills() {
		battleController.Log = "Display skills";

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
		Health = maxHealth;

		baseStrength += (int) Math.Round(baseStrength * strengthIncPerLevel);
	}

	public void DecreaseCooldowns() {
		foreach (Skill skill in skills) {
			skill.DecreaseCooldown();
		}
	}
}
