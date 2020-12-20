using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyAttack : Skill {
    public override void ApplySkillOnSelf(Character attacker) {
        DeadlyAttackBuff buff = null;

        // Finds DeadlyAttackBuff if character already has it.
        foreach (Effect effect in attacker.GetEffects()) {
            if (effect.GetType() == typeof(DeadlyAttackBuff)) {
                buff = (DeadlyAttackBuff) effect;
                break;
            }
        }

        // If character does not have buff, it is created and added to character.
        if (buff == null) {
            buff = (DeadlyAttackBuff) Instantiate(effects[0], attacker.gameObject.transform);
            attacker.AddEffect(buff);
        }
        
        // Strength of buff is renewed with each use.
        buff.Strength = strength;
        // Skill is needed for buff to know when to activate.
        buff.Skill = this;

        if (buff.charges < buff.maxCharges) {
            buff.charges += 1;
            battleController.Log = $"Deadly Attack now has {buff.charges} charge(s)";
        }
        else {
            battleController.Log = $"Deadly Attack has {buff.maxCharges} charges. Use another attack to use charges.";
            cooldown = maxCooldown + 1;
        }
    }

    /// <summary>
    /// This skill is applied only on self, therefore method is empty.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="target"></param>
    public override void ApplySkill(Character attacker, Character target) { }

    public override List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
        return new List<Character>();
    }

    /// <summary>
    /// Cooldown of this skill is set only after maximum charges are stacked or when DeadlyAttackBuff is deactivated.
    /// </summary>
    public override void ResetCooldown() { }
}
