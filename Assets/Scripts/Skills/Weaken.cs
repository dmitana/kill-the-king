using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaken : Skill
{
    public override void ApplySkill(Character attacker, Character target) {
        WeakenDebuff effect = (WeakenDebuff) Instantiate(effects[0], target.gameObject.transform);
        effect.Strength = strength;
        target.Defence -= strength;
        target.AddEffect(effect);
        battleController.Log = $"{attacker} used {skillName} on {target}, which will receive {100 * strength} %" +
                               $" greater damage when wounded next time";
    }
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}
