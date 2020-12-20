using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionDebuff : Effect {
    private int originalStrength = 0;
    
    /// <summary>
    /// Decreases strength of attacker before attack.
    /// </summary>
    /// <param name="c">Attacker.</param>
    /// <param name="s">Used skill.</param>
    public override void Activate(Character c, Skill s) {
        if (duration == 1) {
            originalStrength = c.baseStrength;
            c.baseStrength = (int) (c.baseStrength * Strength);
            duration = 0;
        }
    }

    /// <summary>
    /// Restores strength of attacker after confused attack.
    /// </summary>
    /// <param name="c">Attacker.</param>
    public override void Deactivate(Character c) {
        c.baseStrength = originalStrength;
    }
}
