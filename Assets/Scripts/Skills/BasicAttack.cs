﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength;
        damage = target.DecreaseHealth(damage);
        battleController.Log = $"{attacker} used {skillName} and dealt {damage} damage to {target}";
    }
}