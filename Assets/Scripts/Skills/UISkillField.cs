using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillField : MonoBehaviour {
    private Skill skill;
    private BattleController battleController;

    public void SetSkill(Skill s) {
        skill = s;
    }
    
    public void OnMouseDown() {
        if (battleController.GetChosenSkill() == null)
            battleController.SetChosenSkill(skill);
    }
    
    public void SetBattleController(BattleController controller) {
        battleController = controller;
    }
}
