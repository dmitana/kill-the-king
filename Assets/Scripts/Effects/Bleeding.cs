using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding : Effect {
    public double damagePerRound;

    public override void AtRoundEnd(Character c) {
        c.DecreaseHealth((int) (Strength * damagePerRound));
        duration -= 1;
    }

    public override void Deactivate(Character c) { }
}
