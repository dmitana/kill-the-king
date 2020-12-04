using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
		characterHealthText.text = $"Health: {character.health}";
		characterStrenghtText.text = $"Strength: {character.baseStrength}";

		// Create UI for each skill
		if (character.availableSkills.Count != skillsUI.Count)
			throw new System.ArgumentException(
				$"Character must have {skillsUI.Count} skill, but has {character.availableSkills.Count}"
			);

		for (int i = 0;  i < skillsUI.Count; ++i) {
			var skillTexts = skillsUI[i].GetComponentsInChildren<TMP_Text>();
			skillTexts[0].text = character.availableSkills[i].skillName;
			skillTexts[1].text = character.availableSkills[i].description;

			Toggle toggle = skillsUI[i].GetComponentInChildren<Toggle>();

			if (character.skills.Contains(character.availableSkills[i])) {
				toggle.group = null;
				toggle.isOn = true;
				toggle.interactable = false;
			}
			else {
				int n = i;
				toggle.onValueChanged.AddListener(
					(change) => AddRemoveSkill(change, character.availableSkills[n], character)
				);
			}
		}
	}

	private void AddRemoveSkill(bool change, Skill skill, Character character) {
		if (change)
			character.AddSkill(skill);
		else
			character.RemoveSkill(skill);
	}
}
