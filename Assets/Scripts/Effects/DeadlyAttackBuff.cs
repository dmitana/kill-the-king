using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyAttackBuff : Effect {
    public int charges;
    public int maxCharges;
    private int originalStrength = 0;
    public DeadlyAttack skill;
    public override void AtRoundEnd(Character c) { }

    public override void Activate(Character c) {
        if (duration == 0)
            return;

        originalStrength = c.baseStrength;
        c.baseStrength = (int) ((Strength * c.baseStrength) * (charges + 1));
        charges = 0;
    }

    public override void Deactivate(Character c) {
        if (originalStrength != 0)
            c.baseStrength = originalStrength;
        skill.cooldown = skill.maxCooldown;
        duration = 0;
    }
}
