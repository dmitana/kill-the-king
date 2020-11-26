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
        gameObject.GetComponentInChildren<Image>().color = Color.green;
    }
    
    public void ChooseSkill() {
        if (battleController.GetChosenSkill() == null)
            battleController.SetChosenSkill(skill);
        Debug.Log("Skill chosen");
    }
    
    public void SetBattleController(BattleController controller) {
        battleController = controller;
    }
}
