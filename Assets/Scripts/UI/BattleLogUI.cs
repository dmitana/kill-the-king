using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLogUI : MonoBehaviour {
    public TMP_Text battleLogText;
    private BattleController battleController;
    private String log;

    private void Awake() {
        battleController = GameMaster.instance.GetComponent<BattleController>();
    }
    
    /// <summary>
    /// Registers battle controller's onTurnEnd event.
    /// </summary>
    private void OnEnable() {
        battleController.onTurnEnd += EmptyLine;
    }

    /// <summary>
    /// Unregisters battle controller's onTurnEnd event.
    /// </summary>
    private void OnDisable() {
        battleController.onTurnEnd -= EmptyLine;
    }

    private void EmptyLine(BattleController bc) {
        battleLogText.text += "\n";
    }

    /// <summary>
    /// Obtains oldest log and adds it to text field.
    /// </summary>
    private void Update() {
        log = battleController.Log;
        if (log != null) {
            battleLogText.text += log + "\n";
        }
    }
}
