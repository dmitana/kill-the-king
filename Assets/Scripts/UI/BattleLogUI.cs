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
    /// Obtains oldest log and adds it to text field.
    /// </summary>
    private void Update() {
        log = battleController.Log;
        if (log != null) {
            battleLogText.text += log + "\n\n";
        }
    }
}
