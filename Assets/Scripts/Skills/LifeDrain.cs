using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrain : Skill {
    public override void ApplySkill(Character attacker, Character target) {
		int damage = (int) Math.Round(attacker.baseStrength * strength);
		damage = target.DecreaseHealth(damage);
		attacker.IncreaseHealth(damage);
		battleController.Log = $"{attacker} used {skillName} and stole {damage} HP from {target}";
    }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
	    return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}
