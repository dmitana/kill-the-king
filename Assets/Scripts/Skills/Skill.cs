﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public abstract class Skill : MonoBehaviour {
    public String skillName;
	public String description;
	public String defaultInfo = "After use, its strength increases by {0:D}% up to {1:D}%. Cooldown is {2:D} turn(s).";
    public String Description
	{
		get
		{
			var info = String.Format(defaultInfo, (int) (increasePerUse * 100),
									 (int) (maxStrength * 100), maxCooldown);
			var desc = String.Format(description, (int) (strength * 100));
			return $"{desc} {info}";
		}
	}

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
        List<Effect> buffsToDeactivate = attacker.ProcessBuffs(this);

		if (rnd.NextDouble() < attacker.hitChance) {
			ApplySkillOnSelf(attacker);

			foreach (var target in targets) {
				ApplySkill(attacker, target);
			}

			Improve();
		}

		foreach (Effect buff in buffsToDeactivate) {
			buff.Deactivate(attacker);
		}

        ResetCooldown();
    }

    public virtual void ResetCooldown() {
	    if (maxCooldown > 0)
			cooldown = maxCooldown + 1;
    }

    public abstract void ApplySkill(Character attacker, Character target);

    public virtual void ApplySkillOnSelf(Character c) {}

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
