using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyAttack : Skill {
    public override void PrepareSkill(Character attacker) {
        DeadlyAttackBuff buff = null;

        foreach (Effect effect in attacker.GetEffects()) {
            if (effect.GetType() == typeof(DeadlyAttackBuff)) {
                buff = (DeadlyAttackBuff) effect;
                break;
            }
        }

        if (buff == null) {
            buff = (DeadlyAttackBuff) Instantiate(effects[0], attacker.gameObject.transform);
            attacker.AddEffect(buff);
        }
        
        buff.Strength = strength;

        if (buff.charges < buff.maxCharges)
            buff.charges += 1;
        else {
            battleController.Log = $"Deadly Attack has {buff.maxCharges} charges. Use another attack to use charges. ";
            canBeUsed = false;
        }
    }

    public override void ApplySkill(Character attacker, Character target) { }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        battleController.Log = "Deadly Attack serves as buff skill. It has no targets.";
        return new List<Character>();
    }
}
