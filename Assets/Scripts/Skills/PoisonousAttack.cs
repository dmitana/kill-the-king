using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) {
		int damage = attacker.baseStrength / 2;
        target.DecreaseHealth(damage);
		DamageOverTime effect = (DamageOverTime) Instantiate(effects[0], target.gameObject.transform);
		effect.Strength = attacker.baseStrength;
		effect.damagePerRound = strength;
		target.AddEffect(effect);
	}

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only enemies are valid targets.";
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}
