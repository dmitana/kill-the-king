using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Controls characters' detail UI displaying detail of player's characters.
/// This UI also serves as UI for adding new skills to characters.
/// </summary>
public class CharacterDetailUI : MonoBehaviour {
	public List<GameObject> skillsUI;
	public TMP_Text characterNameText;
	public TMP_Text characterHealthText;
	public TMP_Text characterStrenghtText;

	void Awake() {
		var texts = gameObject.GetComponentsInChildren<TMP_Text>();
		characterNameText = texts[0];
		characterHealthText = texts[1];
		characterStrenghtText = texts[2];
	}

	/// <summary>
	/// Shows characters detail UI for a character.
	///
	/// Health, strength and skills are displaying for the character.
	///
	/// If character the character has some skill point, then skill's
	/// toggle is interactable and skill can be added.
	/// </summary>
	/// <param name="character">Character to show details of.</param>
	public void Show(Character character) {
		characterNameText.text = character.characterName;
		characterHealthText.text = $"Health: {character.maxHealth}";
		characterStrenghtText.text = $"Strength: {character.baseStrength}";

		// Create UI for each skill
		if (character.availableSkills.Count != skillsUI.Count)
			throw new System.ArgumentException(
				$"Character must have {skillsUI.Count} skill, but has {character.availableSkills.Count}"
			);

		for (int i = 0;  i < skillsUI.Count; ++i) {
			var skillTexts = skillsUI[i].GetComponentsInChildren<TMP_Text>();
			skillTexts[0].text = character.availableSkills[i].skillName;

			Toggle toggle = skillsUI[i].GetComponentInChildren<Toggle>();

			var skillsNames = character.skills.Select(x => x.skillName);
			if (skillsNames.Contains(character.availableSkills[i].skillName)) {
				var skill = character.skills.Find(x => x.skillName ==character.availableSkills[i].skillName);
				skillTexts[1].text = skill.Description;
				toggle.group = null;
				toggle.isOn = true;
				toggle.interactable = false;
				continue;
			}
			skillTexts[1].text = character.availableSkills[i].Description;

			if (character.SkillPoints == 0) {
				toggle.interactable = false;
				continue;
			}

			int n = i;
			toggle.onValueChanged.AddListener(
				(change) => AddRemoveSkill(change, character.availableSkills[n], character)
			);
		}
	}

	/// <summary>
	/// Adds or removes skill to/from a character based on toggle change.
	/// </summary>
	/// <param name="change">Change of skill's toggle.</param>
	/// <param name="skill">Skill to be added or removed to/from a character.</param>
	/// <param name="character">Character displayed in a detail.</param>
	private void AddRemoveSkill(bool change, Skill skill, Character character) {
		if (change)
			character.AddSkill(skill);
		else
			character.RemoveSkill(skill);
	}
}
