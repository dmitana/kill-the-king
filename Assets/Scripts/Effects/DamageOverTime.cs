using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : Effect {
    public double damagePerRound;

    public override void AtRoundEnd(Character c) {
        c.DecreaseHealth((int) Math.Round(Strength * damagePerRound));
        duration -= 1;
    }

    public override void Deactivate(Character c) { }
}
