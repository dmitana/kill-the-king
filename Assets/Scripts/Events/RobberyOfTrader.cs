using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberyOfTrader : Event {
	public override void OnAccept() {
		if (rnd.NextDouble() < 0.5f) {
			Success();
		}
		else {
			GenerateEnemies();
			MoveToBattle();
		}
	}
}
