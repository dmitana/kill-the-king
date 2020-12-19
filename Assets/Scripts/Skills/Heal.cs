using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill {
    public override void ApplySkill(Character attacker, Character target) {
		int val = (int) Math.Round(target.maxHealth * strength);
		target.IncreaseHealth(val);
        battleController.Log = $"{attacker} used {skillName} and increased HP of {target} by {val}";
    }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return (playerTeamTurn) ? playerTeam.Characters : enemyTeam.Characters;
    }
}
