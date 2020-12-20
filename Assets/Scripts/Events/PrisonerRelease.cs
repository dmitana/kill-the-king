using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents Prisoner Release event.
/// </summary>
public class PrisonerRelease : Event {
	public KingWeakening effect;

	private BattleController battleController;

	protected override void Initialize() {
		battleController = GameMaster.instance.GetComponent<BattleController>();
	}

	protected override String ModifyEventMessage() {
		return String.Format(eventMessage, (int) (effect.healthDecrease * 100));
	}

	/// <summary>
	/// Randomly determines whether a player succeeded or not.
	///
	/// If the player succeeded, then KingWeakening effect is added
	/// to the global battle controller's effects to weaken the king in
	/// the final battle.
	/// </summary>
	public override void OnAccept() {
		if (rnd.NextDouble() < successRate) {
			var effectInstantiated = Instantiate(effect, battleController.transform);
			battleController.GlobalEffects.Add(effectInstantiated);
			Success();
		}
		else {
			GenerateEnemies();
			MoveToBattle();
		}
	}
}
