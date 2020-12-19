using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectiveShield : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        target.Defence += strength;
        Effect effect = Instantiate(effects[0], target.gameObject.transform);
        target.AddEffect(effect);
        battleController.Log = $"{attacker} used {skillName} and increased {target}'s defence by {100 * strength} %" +
                               $" for {effect.duration} rounds";
    }
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return (playerTeamTurn) ? playerTeam.Characters : enemyTeam.Characters;
    }
}