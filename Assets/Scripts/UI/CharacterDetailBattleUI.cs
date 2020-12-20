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
    private bool charactersObtained = false;
    private BattleController battleController;

    private void Awake() {
        battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
    }

    /// <summary>
    /// When battle is initialized, characters are obtained and their events are registered. Other methods cannot be
    /// used because enemies are not instantiated instantly.
    /// </summary>
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

    /// <summary>
    /// When deactivated, events of all characters are unregistered.
    /// </summary>
    void OnDisable() {
        foreach (Character c in characters) {
            c.onHover -= Show;
            c.onExit -= Hide;
        }
		charactersObtained = false;
    }

    /// <summary>
    /// Activates hover area and fills it with information.
    /// </summary>
    /// <param name="c">Character whose information are filled into hover.</param>
    private void Show(Character c) {
        characterDetail.SetActive(true);
        name.text = c.characterName;
        health.text = $"Health: {c.Health}";
        damage.text = $"Damage: {c.baseStrength}";
        alreadyPlayed.text = $"Already played: {((c.Team.PlayedCharacters.Contains(c)) ? "Yes" : "No")}";
        alreadyRevived.text = c.playable ? $"Already revived: {((c.alreadyRevived) ? "Yes" : "No")}" : "";
        skills.text = "";
        foreach (Skill s in c.skills) {
            skills.text += $"{s.skillName}: {((s.cooldown > 0) ? $"On cooldown for {s.cooldown} rounds" : "Ready")}\n";
        }
    }

    /// <summary>
    /// Deactivates hover area.
    /// </summary>
    /// <param name="c">Character whose information were filled into hover.</param>
    private void Hide(Character c) {
        characterDetail.SetActive(false);
    }
}
