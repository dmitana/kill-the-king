using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantInDanger : Event {
    void Start() {
		description = "Peasant in Danger description";
    }

    protected override void OnAcceptButtonClick() {
		MoveToBattle("Battle");
    }
}
