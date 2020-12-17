using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        var damage = attacker.baseStrength;
        target.DecreaseHealth(damage);
        if (rnd.NextDouble() < strength) {
            if (target.Team.PlayedCharacters.Contains(target))
                target.IsStunned = true;
            else {
                target.Team.AddPlayedCharacter(target);
                target.Team.RemoveUnplayedCharacter(target);
            }
            
            if (target.Team.UnplayedCharacters.Count == 0)
                battleController.SkipTurn = true;
        }
    }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = $"Number of targets: {numOfTargets}";
        battleController.Log = "Only enemies are valid targets.";
        return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }
}