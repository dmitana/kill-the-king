using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormOfKnives : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = (int) Math.Round(attacker.baseStrength * strength);
        damage = target.DecreaseHealth(damage);
        battleController.Log = $"{attacker} used {skillName} and dealt {damage} to {target}";
    }
}