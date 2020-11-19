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

    public void AddEffect(Effect effect) {
        activeEffects.Add(effect);
    }

    public void DecreaseHealth(int damage) {
        health -= damage;
        if (health <= 0)
	        Destroy(this);
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

	public void SetBattleController(BattleController controller) {
		battleController = controller;
	}
	
	private void OnMouseDown() {
		if (battleController.GetChosenCharacter() == null) {
			if (battleController.GetCurrentTeam().GetPlayedCharacters().Contains(this)) {
				Debug.Log($"Character {characterName} was already used.");
				return;
			}

			battleController.SetChosenCharacter(this);
		}
		else
			battleController.AddTarget(this);
	}

	public void DisplaySkills() {
		GameObject skillField = GameObject.FindGameObjectWithTag("SkillField");
		skillField.SetActive(true);
		
		UISkillField[] skillFields = skillField.transform.GetComponentsInChildren<UISkillField>();
		for (int i = 0; i < availableSkills.Count; i++) {
			skillFields[i].SetSkill(availableSkills[i]);
		}
	}
}
