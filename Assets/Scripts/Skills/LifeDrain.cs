using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrain : Skill {
    public override void ApplySkill(Character attacker, Character target) {
		int damage = (int) Math.Round(attacker.baseStrength * strength);
		damage = target.DecreaseHealth(damage);
		attacker.IncreaseHealth(damage);
	}

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only enemies are valid targets.";
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}
