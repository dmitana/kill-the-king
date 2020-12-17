using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantInDanger : Event {
	protected override void Initialize() {
		int idxToRemove = rnd.Next(0, possibleEnemies.Count);
		possibleEnemies.RemoveAt(idxToRemove);
		GenerateEnemies();
	}

    public override void OnAccept() {
		MoveToBattle(false);
    }
}
