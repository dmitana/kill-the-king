using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill {
    public override void ApplySkill(Character attacker, Character target) {
		int val = (int) Math.Round(target.maxHealth * strength);
		target.IncreaseHealth(val);
    }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only player characters are valid targets.";
        return (playerTeamTurn) ? playerTeam.Characters : enemyTeam.Characters;
    }
}
