using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : Event {
	public override void OnReject() {
		OnClose();
	}

    public override void OnAccept() {
		MoveToBattle();
    }
}
