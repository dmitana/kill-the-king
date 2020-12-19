using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fury : Skill {
    public override void ApplySkillOnSelf(Character c) {
        Effect effect = Instantiate(effects[0], c.gameObject.transform);
        effect.Strength = strength;
        c.AddEffect(effect);
        battleController.Log = $"{c} used {skillName} and now won't feel damage for the next {effect.duration} rounds";
    }

    public override void ApplySkill(Character attacker, Character target) { }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return new List<Character>();
    }
}