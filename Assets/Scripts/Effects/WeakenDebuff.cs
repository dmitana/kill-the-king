using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenDebuff : Effect {
    public int OriginalHealth { get; set; }
    public override void AtRoundEnd(Character c) { }

    public override void Activate(Character c, Skill s) { }

    public override void Deactivate(Character c) {
        if (c.Health < OriginalHealth && duration == -1) {
            c.Defence += Strength;
            duration = 0;
        }
    }
}
