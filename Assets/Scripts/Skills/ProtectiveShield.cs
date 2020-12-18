using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectiveShield : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        target.Defence += strength;
        Effect effect = Instantiate(effects[0], target.gameObject.transform);
        target.AddEffect(effect);
    }
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only player characters are valid targets.";
        return (playerTeamTurn) ? playerTeam.Characters : enemyTeam.Characters;
    }
}