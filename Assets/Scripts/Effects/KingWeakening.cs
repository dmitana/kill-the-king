using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingWeakening : Effect {
	public float healthDecrease;

	/// <summary>
	/// Decreases health of king in team as a result of successful event.
	/// </summary>
	/// <param name="team">Team containing king.</param>
	/// <returns>If king's health was decreased or not.</returns>
	public override bool GlobalApply(Team team) {
		foreach (Character character in team.Characters) {
			foreach (Transform child in character.transform) {
				if (child.gameObject.tag == "King") {
					character.DecreaseHealth((int) (character.maxHealth * healthDecrease));
					return true;
				}
			}
		}
		return false;
	}

}
