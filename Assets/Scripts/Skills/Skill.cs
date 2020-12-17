﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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
    protected Random rnd;

    private void Awake() {
        battleController = GameMaster.instance.GetComponent<BattleController>();
        rnd = new Random();
        InstantiateEffects();
    }

	private void InstantiateEffects() {
		for (int i = 0; i < effects.Count; i++) {
			effects[i] = Instantiate(effects[i], transform);
		}
	}

    // Default method used whether skill has multiple targets or not
    public void ApplySkill(Character attacker, List<Character> targets) {
        PrepareSkill(attacker);
        List<Effect> buffsToDeactivate = attacker.ProcessBuffs(this);
        
        foreach (var target in targets) {
            ApplySkill(attacker, target);
        }

        foreach (Effect buff in buffsToDeactivate) {
            buff.Deactivate(attacker);
        }
        Improve();
        ResetCooldown();
    }

    public virtual void ResetCooldown() {
        cooldown = maxCooldown;
    }

    public abstract void ApplySkill(Character attacker, Character target);
    
    public virtual void PrepareSkill(Character c) {}

    public abstract List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn);

    public int GetNumOfTargets() {
        return numOfTargets;
    }

    public void DecreaseCooldown() {
        if (cooldown > 0)
            cooldown--;
    }

	public void Improve(int times = 1) {
        strength += increasePerUse * times;
		strength = (strength > maxStrength) ? maxStrength : strength;
	}
}
