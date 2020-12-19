using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blindness : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        BlindnessDebuff effect = (BlindnessDebuff) Instantiate(effects[0], target.gameObject.transform);
        target.AddEffect(effect);
    }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only enemies are valid targets.";
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}
