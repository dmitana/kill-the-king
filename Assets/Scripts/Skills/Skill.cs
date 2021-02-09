using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Abstract class representing skill. All skills inherit from it.
/// </summary>
public abstract class Skill : MonoBehaviour {
    public String skillName;
	public String description;
	
	/// <summary>
	/// Information how skill changes and how long cooldown it has.
	/// </summary>
	public String defaultInfo = "After use, its strength increases by {0:D}% up to {1:D}%. Cooldown is {2:D} turn(s).";
    
	/// <summary>
	/// Formatted skill description with added defaultInfo. Used in team selection and skill hover.
	/// </summary>
	public String Description
	{
		get
		{
			var info = String.Format(defaultInfo, (int) (increasePerUse * 100),
									 (int) (maxStrength * 100), maxCooldown);
			var desc = String.Format(description, (int) (strength * 100));
			return $"{desc} {info}";
		}
	}

	/// <summary>
	/// Skill strength. Its exact function is different for each skill.
	/// </summary>
    public double strength;
    public double maxStrength;
    public double increasePerUse;

	/// <summary>
	/// Current skill cooldown.
	/// </summary>
    public int cooldown;
	
	/// <summary>
	/// Max skill cooldown.
	/// </summary>
    public int maxCooldown;

	/// <summary>
	/// List of effects skill can apply to its target.
	/// </summary>
    public List<Effect> effects;
	public int numOfTargets;
	
	/// <summary>
	/// Reference to BattleController component of GameMaster GameObject.
	/// </summary>
    protected BattleController battleController;
    protected Random rnd;

    /// <summary>
    /// Skill initialization.
    /// </summary>
    private void Awake() {
        battleController = GameMaster.instance.GetComponent<BattleController>();
        rnd = new Random();
        InstantiateEffects();
    }

    /// <summary>
    /// Effect prefabs have to be instantiated in order to not overwrite prefab.
    /// </summary>
	private void InstantiateEffects() {
		for (int i = 0; i < effects.Count; i++) {
			effects[i] = Instantiate(effects[i], transform);
		}
	}

    /// <summary>
    /// Applies skill to self and targets. Skills without target are applied to self and skills with targets are applied
    /// only to targets. After skill is used, it is improved. Also buffs can affect skill.
    /// </summary>
    public void ApplySkill(Character attacker, List<Character> targets) {
        List<Effect> buffsToDeactivate = attacker.ProcessBuffs(this);

		if (rnd.NextDouble() < attacker.hitChance) {
			ApplySkillOnSelf(attacker);

			foreach (var target in targets) {
				ApplySkill(attacker, target);
				if (target.Health == 0)
					battleController.Log = $"{target.characterName} died.";
			}

			Improve();
		}

		foreach (Effect buff in buffsToDeactivate) {
			buff.Deactivate(attacker);
		}

        ResetCooldown();
    }

    /// <summary>
    /// Sets current cooldown to max cooldown. Because cooldown decrements at round end, max cooldown is increased by 1
    /// to prevent skills with max cooldown 1 to be used each round.
    /// </summary>
    public virtual void ResetCooldown() {
	    if (maxCooldown > 0)
			cooldown = maxCooldown + 1;
    }

    /// <summary>
    /// Applies skill to one target.
    /// </summary>
    /// <param name="attacker">Character which uses skill.</param>
    /// <param name="target">Character on which skill is used.</param>
    public abstract void ApplySkill(Character attacker, Character target);

    /// <summary>
    /// Applies skill to self. Some skills have no effect on caster, therefore method body is empty.
    /// </summary>
    /// <param name="c">Character to which skill is applied.</param>
    public virtual void ApplySkillOnSelf(Character c) {}

    /// <summary>
    /// Highlights valid targets of a skill.
    /// </summary>
    /// <param name="playerTeam"></param>
    /// <param name="enemyTeam"></param>
    /// <param name="playerTeamTurn"></param>
    /// <returns>List of valid targets.</returns>
    public virtual List<Character> HighlightTargets(Team playerTeam, Team enemyTeam, bool playerTeamTurn) {
	    return (playerTeamTurn) ? enemyTeam.Characters : playerTeam.Characters;
    }

    public int GetNumOfTargets() {
        return numOfTargets;
    }

    public void DecreaseCooldown() {
        if (cooldown > 0)
            cooldown--;
    }

    /// <summary>
    /// Increases skill strength.
    /// </summary>
    /// <param name="times">How many times skill was used.</param>
	public void Improve(int times = 1) {
        strength += increasePerUse * times;
		strength = (strength > maxStrength) ? maxStrength : strength;
	}
}
