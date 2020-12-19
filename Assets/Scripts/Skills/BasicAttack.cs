using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength;
        damage = target.DecreaseHealth(damage);
        battleController.Log = $"{attacker} used {skillName} and dealt {damage} to {target}";
    }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}