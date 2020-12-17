using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberyOfTrader : Event {
	public override void OnAccept() {
		if (rnd.NextDouble() < successRate) {
			Success();
		}
		else {
			GenerateEnemies();
			MoveToBattle();
		}
	}
}
