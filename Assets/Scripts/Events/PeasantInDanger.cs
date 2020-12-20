using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents Peasant in Danger event.
/// </summary>
public class PeasantInDanger : Event {
	/// <summary>
	/// Randomly removes one of the possible enemy and generate enemies from the other.
	/// </summary>
	protected override void Initialize() {
		int idxToRemove = rnd.Next(0, possibleEnemies.Count);
		possibleEnemies.RemoveAt(idxToRemove);
		GenerateEnemies();
	}

    public override void OnAccept() {
		MoveToBattle();
    }
}
