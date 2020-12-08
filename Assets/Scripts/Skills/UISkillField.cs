using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillField : MonoBehaviour {
    private Skill skill;
    private BattleController battleController;

    public void SetSkill(Skill s) {
        skill = s;
        gameObject.GetComponentInChildren<Image>().color = (s.cooldown == 0)? Color.green : Color.red;
    }
    
    public void ChooseSkill() {
        if (battleController.ChosenSkill == null && battleController.ChosenCharacter != null) {
            battleController.ChosenSkill = skill;
            Debug.Log("Skill chosen");
        }
    }

    public void Clear() {
        gameObject.GetComponentInChildren<Image>().color = Color.black;
    }
    
    public void SetBattleController(BattleController controller) {
        battleController = controller;
    }
}
