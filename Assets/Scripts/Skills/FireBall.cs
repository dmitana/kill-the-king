using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill {
    public override void ApplySkill(Character attacker, Character target) {
		int damage = attacker.baseStrength;
        damage = target.DecreaseHealth(damage);
        battleController.Log = $"{attacker} used {skillName} and dealt {damage} to {target}";
        if (rnd.NextDouble() < strength && target != null) {
            Effect effect = Instantiate(effects[0], target.gameObject.transform);
            effect.Strength = attacker.baseStrength;
            target.AddEffect(effect);
            battleController.Log = $"{target} is now burning for the next {effect.duration} rounds";
        }
	}
}
