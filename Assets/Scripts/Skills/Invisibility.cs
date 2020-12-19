using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : Skill {
    public override void ApplySkillOnSelf(Character c) {
        if (rnd.NextDouble() < strength) {
            Effect effect = Instantiate(effects[0], c.gameObject.transform);
            c.AddEffect(effect);
            effect.Activate(c, this);
            battleController.Log = $"{c} used {skillName} and now won't be attacked until its next turn";
        }
        else {
            battleController.Log = $"{c} failed to use {skillName}";
        }
    }

    public override void ApplySkill(Character attacker, Character target) { }
    
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return new List<Character>();
    }
}