using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryBuff : Effect {
    private int accumulatedDamage = 0;
    public override void AtRoundEnd(Character c) {
        duration -= 1;
    }

    public override void AfterDamage(Character c, int damage) {
        c.Health += damage;
        accumulatedDamage += (int)((1 + Strength) * damage);
    }

    public override void Deactivate(Character c) {
        if (duration == 0) {
            c.Health -= accumulatedDamage;
            accumulatedDamage = 0;
        }
    }
}
