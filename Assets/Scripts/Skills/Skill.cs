using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
    public String skillName;
    public String description;

    public int strength;
    public int maxStrength;
    public int increasePerUse;

    public int cooldown;
    public int maxCooldown;

    public List<Effect> effects;

    public int numOfTargets;

    void Start() {
        effects = new List<Effect>();
    }

    // Some skills can have multiple target while others only one
    public void ApplySkill(Character attacker, List<Character> targets) {
        foreach (var target in targets) {
            ApplySkill(attacker, target);
        }
    }

    public virtual void ApplySkill(Character attacker, Character target) {
        Debug.Log("Skill main class");
    }

    public virtual List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        Debug.Log("Skill main class");
        return new List<Character>();
    }

    public int GetNumOfTargets() {
        return numOfTargets;
    }
}