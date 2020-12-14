using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterDetailBattleUI : MonoBehaviour
{
    public GameObject characterDetail;
    public TMP_Text name;
    public TMP_Text health;
    public TMP_Text damage;
    public TMP_Text alreadyPlayed;
    public TMP_Text alreadyRevived;
    public TMP_Text skills;
    
    private List<Character> characters;
    private bool charactersObtained;
    private BattleController battleController;

    private void Awake() {
        battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
        charactersObtained = false;
    }

    private void Update() {
        if (!charactersObtained && battleController.battleReady) {
            characters = new List<Character>();
            GameObject[] charactersGO = GameObject.FindGameObjectsWithTag("Character");

            foreach (GameObject go in charactersGO) {
                Character c = go.GetComponent<Character>();
                c.onHover += Show;
                c.onExit += Hide;
                characters.Add(c);
            }
            charactersObtained = true;
        }
    }

    void OnDisable() {
        foreach (Character c in characters) {
            c.onHover -= Show;
            c.onExit -= Hide;
        }
    }

    private void Show(Character c) {
        characterDetail.SetActive(true);
        name.text = c.characterName;
        health.text = $"Health: {c.Health}";
        damage.text = $"Damage: {c.baseStrength}";
        alreadyPlayed.text = $"Already played: {((c.Team.UnplayedCharacters.Contains(c)) ? "Yes" : "No")}";
        alreadyRevived.text = c.playable ? $"Already revived: {((c.alreadyRevived) ? "Yes" : "No")}" : "";
        skills.text = "";
        foreach (Skill s in c.skills) {
            skills.text += $"{s.skillName}: {((s.cooldown > 0) ? $"On cooldown for {s.cooldown} rounds" : "Ready")}\n";
        }
    }

    private void Hide(Character c) {
        characterDetail.SetActive(false);
    }
}
