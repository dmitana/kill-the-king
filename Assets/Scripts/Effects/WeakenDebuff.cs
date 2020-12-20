using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenDebuff : Effect {
    private bool receivedDamage = false;
    
    public override void Deactivate(Character c) {
        if (receivedDamage && duration == 1) {
            c.Defence += Strength;
            duration = 0;
        }
    }

    public override void AfterDamage(Character c, int damage) {
        receivedDamage = true;
        Deactivate(c);
    }
}
