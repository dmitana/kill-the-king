﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength;
        damage = target.DecreaseHealth(damage);
        battleController.Log = $"{attacker} used {skillName} and dealt {damage} damage to {target}";
        if (rnd.NextDouble() < strength && target != null) {
            // If character was already played, it will skip its next turn after team reset at turn end.
            if (target.Team.PlayedCharacters.Contains(target))
                target.IsStunned = true;
            else {
                target.Team.AddPlayedCharacter(target);
                target.Team.RemoveUnplayedCharacter(target);
            }
            
            // If all characters were played, team will skip its turn.
            if (target.Team.UnplayedCharacters.Count == 0)
                battleController.SkipTurn = true;
            battleController.Log = $"{target} is stunned and cannot be used";
        }
    }
}