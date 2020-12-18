using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : Skill {
    public override void ApplySkillOnSelf(Character c) {
        if (rnd.NextDouble() < strength) {
            Effect effect = Instantiate(effects[0], c.gameObject.transform);
            c.AddEffect(effect);
            effect.Activate(c, this);
        }
    }

    public override void ApplySkill(Character attacker, Character target) { }
    
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = "Deadly Attack serves as buff skill. It has no targets.";
        return new List<Character>();
    }
}