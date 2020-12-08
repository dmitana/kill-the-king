using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength;
        target.DecreaseHealth(damage);
    }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        Debug.Log($"Number of targets: {numOfTargets}");
        Debug.Log("Only enemies are valid targets.");
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}