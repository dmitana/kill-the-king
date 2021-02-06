using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class representing effect. All effects inherit from it.
/// </summary>
public abstract class Effect : MonoBehaviour {
	/// <summary>
	/// Number of rounds or turns (depending on effect) effect affects character.
	/// </summary>
    public int duration;
    public String description;
    public String effectName;
    
    /// <summary>
    /// Strength of effect. Can be damage per round, reduction of damage, ...
    /// </summary>
    public double Strength { get; set; }

    /// <summary>
    /// Effect might perform some action at round end, such as inflicting damage.
    /// </summary>
    /// <param name="c">Character affected by effect.</param>
    public virtual void AtRoundEnd(Character c) { }

    /// <summary>
    /// Effect might perform some action when activated before applying skill to targets or after being added to
    /// character's list of active effects.
    /// </summary>
    /// <param name="c">Character affected by effect.</param>
    /// <param name="s">Skill affected by effect.</param>
    public virtual void Activate(Character c, Skill s) { }

    /// <summary>
    /// Effect might perform some action when deactivated, either when duration is 0 and effect will be removed or after
    /// attack. Some actions might be changing defence to original value as with WeakenDebuff and Protection.
    /// </summary>
    /// <param name="c"></param>
    public virtual void Deactivate(Character c) { }

    /// <summary>
    /// Effect might perform some action after character receives damage, such as deactivating as with WeakenDebuff and
    /// DeadlyAttackBuff.
    /// </summary>
    /// <param name="c">Character which is affected by effect.</param>
    /// <param name="damage">Final damage caused to character.</param>
    public virtual void AfterDamage(Character c, int damage) { }

    /// <summary>
    /// Applies global effect to entire team.
    /// </summary>
    /// <param name="team">Team to which effect will be applied.</param>
    /// <returns>Whether effect was applied or not.</returns>
	public virtual bool GlobalApply(Team team) {
		return true;
	}
}
