using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents battle event.
/// </summary>
public class Battle : Event {
	/// <summary>
	/// Whether battle is final or not. Only last battle with the King is final.
	/// </summary>
	public bool isFinalBattle = false;

	/// <summary>
	/// Generates enemies on initialization because they are used as battle event view.
	/// </summary>
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
