using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill {
    void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength;
        target.DecreaseHealth(damage);
    }
}