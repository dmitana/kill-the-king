using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) {
		int damage = attacker.baseStrength / 2;
        damage = target.DecreaseHealth(damage);
		DamageOverTime effect = (DamageOverTime) Instantiate(effects[0], target.gameObject.transform);
		effect.Strength = attacker.baseStrength;
		effect.damagePerRound = strength;
		target.AddEffect(effect);
		battleController.Log = $"{attacker} used {skillName} and dealt {damage} damage to {target}";
		battleController.Log = $"{target} is poisoned for next {effect.duration} rounds";
	}
}
