using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillFieldUI : MonoBehaviour {
    private Skill skill;
    private BattleController battleController;

    private void Awake() {
        battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
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
}
