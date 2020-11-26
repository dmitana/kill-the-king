using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength;
        target.DecreaseHealth(damage);
    }

    public override void HighlightTargets(Team playerTeam, Team enemyTeam) {
        Debug.Log("All enemies are valid targets.");
    }
}