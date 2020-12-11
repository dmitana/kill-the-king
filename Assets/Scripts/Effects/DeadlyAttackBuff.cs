using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyAttackBuff : Effect {
    public int charges;
    public int maxCharges;
    public override void ApplyEffect(Character c) {
        duration -= 1;
    }

    public override void Deactivate(Character c) { }
}
