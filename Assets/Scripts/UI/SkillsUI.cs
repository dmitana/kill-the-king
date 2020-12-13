using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUI : MonoBehaviour {
    private BattleController battleController;

    private void Awake() {
        battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
    }

    private void Update() {
        if (battleController.ChosenCharacter != null) {
            Character c = battleController.ChosenCharacter;
            SkillFieldUI[] skillFields = transform.GetComponentsInChildren<SkillFieldUI>();
            for (int i = 0; i < c.skills.Count; i++) {
                skillFields[i].SetSkill(c.skills[i]);
            }
        }
    }
}
