using System;
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
    public bool canBeUsed;

    private void Awake() {
        battleController = GameMaster.instance.GetComponent<BattleController>();
        rnd = new Random();
        canBeUsed = true;
		InstantiateEffects();
    }

	private void InstantiateEffects() {
		for (int i = 0; i < effects.Count; i++) {
			effects[i] = Instantiate(effects[i], transform);
		}
	}

    // Default method used whether skill has multiple targets or not
    public virtual void ApplySkill(Character attacker, List<Character> targets) {
        DeadlyAttackBuff buff = null;
        foreach (Effect effect in attacker.GetEffects()) {
            if (effect.GetType() == typeof(DeadlyAttackBuff)) {
                buff = (DeadlyAttackBuff) effect;
                buff.Activate(attacker);
                break;
            }
        }
        
        cooldown = maxCooldown;
        foreach (var target in targets) {
            ApplySkill(attacker, target);
        }

        if (buff != null) {
            buff.Deactivate(attacker);
        }
        strength += increasePerUse;
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
