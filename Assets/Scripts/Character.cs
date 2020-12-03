using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	public string characterName;
	public int health;
	public int baseStrength;

	public List<Skill> availableSkills = new List<Skill>();
	public List<Skill> skills = new List<Skill>();

	private List<Effect> activeEffects = new List<Effect>();
	private BattleController battleController;
	private Team team;
	private bool inBattle = false;

	public void AddEffect(Effect effect) {
		activeEffects.Add(effect);
	}

	public void DecreaseHealth(int damage) {
		health -= damage;
		Debug.Log($"New character health: {health}");
		if (health <= 0) {
			team.RemoveCharacterFromTeam(this);
			Destroy(gameObject);
		}
	}

	public void AddSkill(Skill skill) {
		if (!availableSkills.Contains(skill))
			throw new System.ArgumentException(
				$"Trying to add {skill.name}, which is not available for current character."
			);

		skills.Add(skill);
	}

	public void RemoveSkill(Skill skill) {
		skills.Remove(skill);
	}

	public void InitializeForBattle(BattleController controller) {
		battleController = controller;
		inBattle = true;
	}

	private void OnMouseDown() {
		if (inBattle == false)
			return;

		Team currentTeam = battleController.GetCurrentTeam();
		if (battleController.GetChosenCharacter() == null && currentTeam.GetUnplayedCharacters().Contains(this)) {
			if (battleController.GetCurrentTeam().GetPlayedCharacters().Contains(this)) {
				Debug.Log($"Character {characterName} was already used.");
				return;
			}

			battleController.SetChosenCharacter(this);
		}
		else if (battleController.GetChosenSkill() != null)
			battleController.AddTarget(this);
	}

	public void DisplaySkills() {
		Debug.Log("Display skills");
		GameObject skillField = battleController.GetSkillField();

		UISkillField[] skillFields = skillField.transform.GetComponentsInChildren<UISkillField>();
		for (int i = 0; i < skills.Count; i++) {
			skillFields[i].SetSkill(skills[i]);
		}
	}

	public void SetTeam(Team t) {
		team = t;
	}
}
