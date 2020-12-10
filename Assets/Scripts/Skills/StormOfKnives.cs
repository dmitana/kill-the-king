using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormOfKnives : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = (int) (attacker.baseStrength * strength);
        target.DecreaseHealth(damage);
    }
    
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only enemies are valid targets.";
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}