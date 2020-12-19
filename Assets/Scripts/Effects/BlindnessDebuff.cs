using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindnessDebuff : Effect {
	private bool wasApplied = false;

    public override void Activate(Character c, Skill s) {
		if (wasApplied)
			return;

		Strength = c.hitChance * s.strength;
		c.hitChance -= Strength;
	}

    public override void Deactivate(Character c) {
		if (wasApplied)
			return;

		c.hitChance += Strength;
		duration = 0;
		wasApplied = true;
    }
}
