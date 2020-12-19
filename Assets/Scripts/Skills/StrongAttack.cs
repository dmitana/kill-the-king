﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength * 2;
        damage = target.DecreaseHealth(damage);
        battleController.Log = $"{attacker} used {skillName} and dealt {damage} to {target}";
        if (rnd.NextDouble() < strength) {
            Effect effect = Instantiate(effects[0], target.gameObject.transform);
            effect.Strength = attacker.baseStrength;
            target.AddEffect(effect);
            battleController.Log = $"{target} is bleeding for {effect.duration} rounds";
        }
    }
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}