using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding : Effect {
    public double damagePerRound;

    public override void applyEffect(Character c) {
        c.DecreaseHealth((int) (BaseAttackStrength * damagePerRound));
        duration -= 1;
    }
}
