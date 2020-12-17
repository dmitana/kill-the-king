using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : Event {
	public bool isFinalBattle = false;

	protected override void Initialize() {
		GenerateEnemies();
	}

	public override void OnReject() {
		OnClose();
	}

    public override void OnAccept() {
		MoveToBattle(isFinalBattle);
    }
}
