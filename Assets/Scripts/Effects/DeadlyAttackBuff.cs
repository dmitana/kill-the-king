using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyAttackBuff : Effect {
    public int charges;
    public int maxCharges;
    private int originalStrength = 0;
    public override void AtRoundEnd(Character c) { }

    public override void Activate(Character c, Skill s) {
        if (duration == 0 || typeof(DeadlyAttack) == s.GetType())
            return;

        originalStrength = c.baseStrength;
        c.baseStrength = (int) Math.Round((Strength * c.baseStrength) * (charges + 1));
        charges = 0;
    }

    public override void Deactivate(Character c) {
        if (originalStrength != 0)
            c.baseStrength = originalStrength;
        duration = 0;
    }
}
