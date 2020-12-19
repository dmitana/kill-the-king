using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenDebuff : Effect {
    public int OriginalHealth { get; set; }

    public override void Deactivate(Character c) {
        if (c.Health < OriginalHealth && duration == -1) {
            c.Defence += Strength;
            duration = 0;
        }
    }

    public override void AfterDamage(Character c, int damage) {
        Deactivate(c);
    }
}
