using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerRelease : Event {
	public KingWeakening effect;

	private BattleController battleController;

	protected override void Initialize() {
		battleController = GameMaster.instance.GetComponent<BattleController>();
	}

	public override void OnAccept() {
		if (rnd.NextDouble() < 0.5f) {
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
