using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private SkillFieldUI[] skillFields;

    private void Awake() {
        battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
        skillFields = transform.GetComponentsInChildren<SkillFieldUI>();
    }

    private void OnEnable() {
        battleController.onCharacterChanged += SetSkills;
    }

    private void OnDisable() {
        battleController.onCharacterChanged -= SetSkills;
    }

    /// <summary>
    /// If character was chosen, its skills are filled into skill fields.
    /// </summary>
    private void SetSkills(Character c) {
        for (int i = 0; i < 6; i++) {
            if (i < c.skills.Count) {
                skillFields[i].SetSkill(c.skills[i]);
                skillFields[i].gameObject.GetComponent<Button>().interactable = true;
            }
            else
                skillFields[i].gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
