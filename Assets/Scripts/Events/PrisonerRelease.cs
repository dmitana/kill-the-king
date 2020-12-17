using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerRelease : Event {
	public KingWeakening effect;

	private BattleController battleController;

	protected override void Initialize() {
		battleController = GameMaster.instance.GetComponent<BattleController>();
	}

	protected override String ModifyEventMessage() {
		return String.Format(eventMessage, (int) (effect.healthDecrease * 100));
	}

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
