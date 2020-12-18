using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CharacterDetailUI : MonoBehaviour {
	public List<GameObject> skillsUI;
	public TMP_Text characterNameText;
	public TMP_Text characterHealthText;
	public TMP_Text characterStrenghtText;


	void Awake() {
		// Get character detail UI childen
		var texts = gameObject.GetComponentsInChildren<TMP_Text>();
		characterNameText = texts[0];
		characterHealthText = texts[1];
		characterStrenghtText = texts[2];
	}

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
			skillTexts[1].text = character.availableSkills[i].Description;

			Toggle toggle = skillsUI[i].GetComponentInChildren<Toggle>();

			var skillsNames = character.skills.Select(x => x.skillName);
			if (skillsNames.Contains(character.availableSkills[i].skillName)) {
				toggle.group = null;
				toggle.isOn = true;
				toggle.interactable = false;
				continue;
			}

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

	private void AddRemoveSkill(bool change, Skill skill, Character character) {
		if (change)
			character.AddSkill(skill);
		else
			character.RemoveSkill(skill);
	}
}
