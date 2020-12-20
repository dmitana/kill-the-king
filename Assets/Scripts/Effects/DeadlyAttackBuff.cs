using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyAttackBuff : Effect {
    public int charges;
    public int maxCharges;
    public DeadlyAttack Skill { get; set; }
    
    private int originalStrength = 0;
    private bool buffUsed = false;
    private bool damageReceived = false;

    /// <summary>
    /// If effect was not used already and skill, that is used is not Deadly Attack, original strength of character is
    /// stored and character's base strength is boosted.
    /// </summary>
    /// <param name="c">Character with effect.</param>
    /// <param name="s">Skill that is used.</param>
    public override void Activate(Character c, Skill s) {
        if (buffUsed || typeof(DeadlyAttack) == s.GetType())
            return;

        originalStrength = c.baseStrength;
        c.baseStrength = (int) Math.Round((Strength * c.baseStrength) * (charges + 1));
        charges = 0;
        buffUsed = true;
    }

    /// <summary>
    /// If effect was used or character received damage, original strength is restored if it was set and cooldown is set
    /// if it was not set already. 
    /// </summary>
    /// <param name="c">Character with effect.</param>
    public override void Deactivate(Character c) {
        if (!buffUsed && !damageReceived)
            return;

        if (originalStrength != 0)
            c.baseStrength = originalStrength;

        if (Skill.cooldown == 0)
            Skill.cooldown = Skill.maxCooldown + 1;

        duration = 0;
    }

    /// <summary>
    /// When damage is received, effect is removed.
    /// </summary>
    /// <param name="c">Character with effect.</param>
    /// <param name="damage">Received final damage.</param>
    public override void AfterDamage(Character c, int damage) {
        if (damage != 0) {
            damageReceived = true;
            Deactivate(c);
        }
    }
}
