using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHighlightUI : MonoBehaviour {
    public Character character;
    public Image image;

    private BattleController battleController;

    private void Awake() {
        battleController = GameMaster.instance.GetComponent<BattleController>();
    }

    private void OnEnable() {
        character.onSelected += Selected;
        character.onSkillSelected += Selected;
        character.onValid += Valid;
        character.onInvalid += Invalid;
        battleController.onTurnEnd += TurnEnd;
    }

    private void OnDisable() {
        character.onSelected -= Selected;
        character.onSkillSelected -= Selected;
        character.onValid -= Valid;
        character.onInvalid -= Invalid;
        battleController.onTurnEnd -= TurnEnd;
    }

    private void Selected(bool b) {
        image.color = b ? Color.yellow : Color.green;
    }

    private void Valid(bool b) {
        image.color = b ? Color.green : Color.red;
    }

    private void TurnEnd(BattleController bc) {
        image.color = Color.clear;
    }

    private void Invalid(bool b) {
        image.color = Color.clear;
    }
} 
