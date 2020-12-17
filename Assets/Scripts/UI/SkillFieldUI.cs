using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillFieldUI : MonoBehaviour {
    public Image background;
    public TMP_Text skillName;
    public SkillsUI parent;
    
    private Skill skill;
    private BattleController battleController;

    private void Awake() {
        battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
        background.color = Color.black;
    }

    public void SetSkill(Skill s) {
        skill = s;
        background.color = (s.cooldown == 0)? Color.green : Color.red;
        skillName.text = s.skillName;
    }
    
    public void ChooseSkill() {
        if (battleController.ChosenSkill == null && battleController.ChosenCharacter != null && skill != null) {
            if (skill.cooldown > 0) {
                battleController.Log = $"This skill is on cooldown for {skill.cooldown} rounds";
                return;
            }
            battleController.ChosenSkill = skill;
        }
    }

    public void Clear() {
        gameObject.GetComponentInChildren<Image>().color = Color.black;
    }
    
    public void HoverOn() {
        if (skill != null && battleController.ChosenCharacter != null) {
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

    public void HoverOff() {
        if (skill != null && battleController.ChosenCharacter != null) {
            parent.skillDescription.SetActive(false);
        }
    }
}
