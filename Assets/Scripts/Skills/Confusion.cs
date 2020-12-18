using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        ConfusionDebuff effect = (ConfusionDebuff) Instantiate(effects[0], target.gameObject.transform);
        effect.Strength = strength;
        target.AddEffect(effect);
    }
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only enemies are valid targets.";
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}
