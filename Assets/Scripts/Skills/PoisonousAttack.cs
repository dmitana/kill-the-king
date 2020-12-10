using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousAttack : Skill {
    public override void ApplySkill(Character attacker, Character target) { }
    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        throw new System.NotImplementedException();
    }
}