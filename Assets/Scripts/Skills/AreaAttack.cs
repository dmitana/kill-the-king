using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : Skill
{
    public override void ApplySkill(Character attacker, Character target) {
        var damage = (int) Math.Round(attacker.baseStrength * strength);
        target.DecreaseHealth(damage);
    }
    
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only enemies are valid targets.";
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}
