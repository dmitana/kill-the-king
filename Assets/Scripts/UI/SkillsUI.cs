using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillsUI : MonoBehaviour {
    public GameObject skillDescription;
    public TMP_Text skillName;
    public TMP_Text strength;
    public TMP_Text maxStrength;
    public TMP_Text increasePerUse;
    public TMP_Text cooldown;
    public TMP_Text maxCooldown;
    public TMP_Text numOfTargets;
    public TMP_Text description;

    private BattleController battleController;

    private void Awake() {
        battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
    }

    /// <summary>
    /// If character was chosen, its skills are filled into skill fields.
    /// </summary>
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
