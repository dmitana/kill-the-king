using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	public string characterName;
    public int health;
    public int baseStrength;

	public List<Skill> availableSkills = new List<Skill>();
    public List<Skill> skills = new List<Skill>();

    private List<Effect> _activeEffects = new List<Effect>();

    public void AddEffect(Effect effect) {
        _activeEffects.Add(effect);
    }

    public void DecreaseHealth(int damage) {
        health -= damage;
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
}
