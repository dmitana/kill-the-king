using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantInDanger : Event {
    public override void OnAccept() {
		MoveToBattle(false);
    }
}
