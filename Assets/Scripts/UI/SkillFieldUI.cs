using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillFieldUI : MonoBehaviour {
    private Skill skill;
    private BattleController battleController;
    private SkillsUI parent;

    private void Awake() {
        battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
        parent = transform.parent.GetComponent<SkillsUI>();
    }

    public void SetSkill(Skill s) {
        skill = s;
        gameObject.GetComponentInChildren<Image>().color = (s.cooldown == 0 && s.canBeUsed)? Color.green : Color.red;
    }
    
    public void ChooseSkill() {
        if (battleController.ChosenSkill == null && battleController.ChosenCharacter != null) {
            if (skill.cooldown > 0) {
                battleController.Log = $"This skill is on cooldown for {skill.cooldown} rounds";
                return;
            }

            if (!skill.canBeUsed) {
                battleController.Log = "This skill cannot be used currently.";
                return;
            }
            battleController.ChosenSkill = skill;
        }
    }

    public void Clear() {
        gameObject.GetComponentInChildren<Image>().color = Color.black;
    }
    
    private void OnMouseOver() {
        if (skill != null) {
            parent.skillDescription.SetActive(true);
            parent.skillName.text = skill.skillName;
            parent.strength.text = $"Strength: {skill.strength * 100} %";
            parent.maxStrength.text = $"Max strength: {skill.maxStrength * 100} %";
            parent.increasePerUse.text = $"Increase per use: {skill.increasePerUse * 100} %";
            parent.cooldown.text = $"Cooldown: {skill.cooldown}";
            parent.maxCooldown.text = $"Max cooldown: {skill.maxCooldown}";
            parent.numOfTargets.text =
                $"Num of targets: {((skill.numOfTargets == -1) ? "All" : skill.numOfTargets.ToString())}";
            parent.description.text = skill.description;
        }
    }

    private void OnMouseExit() {
        if (skill != null) {
            parent.skillDescription.SetActive(false);
            parent.skillName.text = "";
            parent.strength.text = "";
            parent.maxStrength.text = "Max strength: ";
            parent.increasePerUse.text = "Increase per use: ";
            parent.cooldown.text = "Cooldown: ";
            parent.maxCooldown.text = "Max cooldown: ";
            parent.numOfTargets.text = "Num of targets: ";
            parent.description.text = "";
        }
    }
}
