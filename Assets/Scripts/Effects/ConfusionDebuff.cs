using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionDebuff : Effect {
    private int originalStrength = 0;
    public override void Activate(Character c, Skill s) {
        if (duration == 1) {
            originalStrength = c.baseStrength;
            c.baseStrength = (int) (c.baseStrength * Strength);
            duration = 0;
        }
    }

    public override void Deactivate(Character c) {
        c.baseStrength = originalStrength;
    }
}
