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

    /// <summary>
    /// Registers battle controller's onTurnEnd event.
    /// </summary>
	private void OnEnable() {
        battleController.onTurnEnd += Clear;
        battleController.onSkillChanged += ResetColor;
    }

    /// <summary>
    /// Unregisters battle controller's onTurnEnd event.
    /// </summary>
    private void OnDisable() {
        battleController.onTurnEnd -= Clear;
        battleController.onSkillChanged -= ResetColor;
    }

    /// <summary>
    /// Sets skill into a skill field and changes color depending on whether skill can be used.
    /// </summary>
    /// <param name="s"></param>
    public void SetSkill(Skill s) {
        skill = s;
        background.color = (s.cooldown == 0)? Color.green : Color.red;
        skillName.text = s.skillName;
    }

    /// <summary>
    /// When player presses button of skill field, skill is set as chosen if some character was already chosen and
    /// skill was not.
    /// </summary>
    public void ChooseSkill() {
        if (battleController.ChosenCharacter != null && skill != null) {
            if (skill != battleController.ChosenSkill) {
                if (skill.cooldown > 0) {
                    battleController.Log = $"This skill is on cooldown for {skill.cooldown} rounds";
                    return;
                }

                if (battleController.ChosenSkill != null)
                    battleController.SkillChanged = true;
                battleController.ChosenSkill = skill;
                background.color = Color.yellow;
            }
        }
    }

    /// <summary>
    /// Clears all skill fields when battle controller emits onTurnEnd event.
    /// </summary>
    /// <param name="bc"></param>
    public void Clear(BattleController bc) {
        background.color = Color.black;
        gameObject.GetComponent<Button>().interactable = false;
        skillName.text = "No Skill";
        HoverOff();
        skill = null;
    }

    /// <summary>
    /// Activates hover area and fills it with skill information.
    /// </summary>
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
            parent.description.text = skill.Description;
        }
    }

    /// <summary>
    /// Deactivates hover area.
    /// </summary>
    public void HoverOff() {
        if (skill != null && battleController.ChosenCharacter != null) {
            parent.skillDescription.SetActive(false);
        }
    }

    private void ResetColor() {
        if (skill != null && battleController.ChosenSkill != skill)
            background.color = (skill.cooldown == 0)? Color.green : Color.red;
    }
}
