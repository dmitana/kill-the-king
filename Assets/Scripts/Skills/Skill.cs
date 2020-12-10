using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour {
    public String skillName;
    public String description;

    public double strength;
    public double maxStrength;
    public double increasePerUse;

    public int cooldown;
    public int maxCooldown;

    public List<Effect> effects;

    public int numOfTargets;
    protected BattleController battleController;

    private void Awake() {
        battleController = GameMaster.instance.GetComponent<BattleController>();
    }

    void Start() {
        effects = new List<Effect>();
    }

    // Default method used whether skill has multiple targets or not
    public void ApplySkill(Character attacker, List<Character> targets) {
        cooldown = maxCooldown;
        foreach (var target in targets) {
            ApplySkill(attacker, target);
        }
    }

    public abstract void ApplySkill(Character attacker, Character target);

    public abstract List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn);

    public int GetNumOfTargets() {
        return numOfTargets;
    }

    public void DecreaseCooldown() {
        if (cooldown > 0)
            cooldown--;
    }
}