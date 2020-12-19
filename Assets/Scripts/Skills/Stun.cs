using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength;
        damage = target.DecreaseHealth(damage);
        battleController.Log = $"{attacker} used {skillName} and dealt {damage} to {target}";
        if (rnd.NextDouble() < strength) {
            if (target.Team.PlayedCharacters.Contains(target))
                target.IsStunned = true;
            else {
                target.Team.AddPlayedCharacter(target);
                target.Team.RemoveUnplayedCharacter(target);
            }
            
            if (target.Team.UnplayedCharacters.Count == 0)
                battleController.SkipTurn = true;
            battleController.Log = $"{target} is stunned and cannot be used";
        }
    }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}