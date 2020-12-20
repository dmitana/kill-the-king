using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InvisibilityBuff : Effect {
    private bool canBeDeactivated = false;
    
    /// <summary>
    /// Invisibility can be deactivated if character used any skill except Invisibility.
    /// </summary>
    /// <param name="c"></param>
    /// <param name="s"></param>
    public override void Activate(Character c, Skill s) {
        if (duration == 1)
            c.IsInvisible = true;
        if (s.GetType() != typeof(Invisibility))
            canBeDeactivated = true;
    }

    public override void Deactivate(Character c) {
        if (canBeDeactivated) {
            c.IsInvisible = false;
            duration = 0;
        }
    }
}
