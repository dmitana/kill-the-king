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
    
    public override void AtRoundEnd(Character c) { }

    public override void Activate(Character c, Skill s) {
        if (duration == 0 || typeof(DeadlyAttack) == s.GetType())
            return;

        originalStrength = c.baseStrength;
        c.baseStrength = (int) Math.Round((Strength * c.baseStrength) * (charges + 1));
        charges = 0;
        buffUsed = true;
    }

    public override void Deactivate(Character c) {
        if (!buffUsed && !damageReceived)
            return;

        if (originalStrength != 0)
            c.baseStrength = originalStrength;

        if (Skill.cooldown == 0)
            Skill.cooldown = Skill.maxCooldown;

        duration = 0;
    }

    public override void AfterDamage(Character c, int damage) {
        if (damage != 0) {
            damageReceived = true;
            Deactivate(c);
        }
    }
}
