using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength * 2;
        damage = target.DecreaseHealth(damage);
        battleController.Log = $"{attacker} used {skillName} and dealt {damage} damage to {target}";
        if (rnd.NextDouble() < strength && target != null) {
            Effect effect = Instantiate(effects[0], target.gameObject.transform);
            effect.Strength = attacker.baseStrength;
            target.AddEffect(effect);
            battleController.Log = $"{target} is bleeding for {effect.duration} rounds";
        }
    }
}