using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill {
    public override void ApplySkill(Character attacker, Character target) {
		int damage = attacker.baseStrength;
        target.DecreaseHealth(damage);
        if (rnd.NextDouble() < strength) {
            Effect effect = Instantiate(effects[0], target.gameObject.transform);
            effect.Strength = attacker.baseStrength;
            target.AddEffect(effect);
        }
	}
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only enemies are valid targets.";
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}
