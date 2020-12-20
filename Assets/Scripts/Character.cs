using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Represents character, which can be controlled by player or enemy.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour {
	/// <summary>
	/// Name of character.
	/// </summary>
	public string characterName;
	
	/// <summary>
	/// Character sprite.
	/// </summary>
	public Sprite characterImg;
	
	/// <summary>
	/// Max health character can have. Increases with each level.
	/// </summary>
	public int maxHealth;
	
	/// <summary>
	/// Strength of basic attack. Increases with each level.
	/// </summary>
	public int baseStrength;
	
	/// <summary>
	/// Chance that using skill will be successful.
	/// </summary>
	public double hitChance = 1.0;
	
	/// <summary>
	/// List of all skills character can have.
	/// </summary>
	public List<Skill> availableSkills = new List<Skill>();
	
	/// <summary>
	/// List of all skills player has chosen from <c>availableSkills</c>.
	/// </summary>
	public List<Skill> skills = new List<Skill>();
	
	/// <summary>
	/// Amount by which <c>maxHealth</c> of AI will increase to increase difficulty when player becomes stronger.
	/// </summary>
	public float healthIncPerLevelAI;
	
	/// <summary>
	/// Amount by which <c>baseStrength</c> of AI will increase to increase difficulty when player becomes stronger.
	/// </summary>
	public float strengthIncPerLevelAI;

	/// <summary>
	/// Represents how many skills can player choose for each character. Increases by 1 after each finished environment.
	/// </summary>
	public int SkillPoints { get; private set; } = 1;
	
	/// <summary>
	/// Team to which character belongs to. Can be <c>playerTeam</c> or <c>enemyTeam</c>.
	/// </summary>
	public Team Team { get; set; }
	
	/// <summary>
	/// Current health of character.
	/// </summary>
	public int Health { get; set; }
	
	/// <summary>
	/// Flag used to control whether clicking on character has any effect. When in battle, is is set to <c>true</c> and
	/// character can be clicked.
	/// </summary>
	public bool InBattle { get; private set; } = false;
	
	/// <summary>
	/// Defence used to reduce/increase damage to character. Default value is 0.
	/// </summary>
	public double Defence { get; set; }
	
	/// <summary>
	/// Flag used to control whether character becomes critically wounded or dies immediately. Only player characters
	/// can be critically wounded.
	/// </summary>
	public bool playable;
	
	/// <summary>
	/// Flag used to signal that character is critically wounded.
	/// </summary>
	public bool isCriticallyWounded = false;
	
	/// <summary>
	/// Flag used to signal if character was already revived. Each character can be revived only once in current battle,
	/// therefore if character was already revived and its health becomes 0, it dies.
	/// </summary>
	public bool alreadyRevived = false;
	
	/// <summary>
	/// Flag used to control whether character can be played next turn.
	/// </summary>
	public bool IsStunned { get; set; } = false;
	
	/// <summary>
	/// Flag used to control whether character can be attacked until its next turn.
	/// </summary>
	public bool IsInvisible { get; set; } = false;

	/// <summary>
	/// List of active effects which affect the character.
	/// </summary>
	private List<Effect> activeEffects = new List<Effect>();
	
	/// <summary>
	/// Reference to <c>BattleController</c> component of GameMaster GameObject.
	/// </summary>
	private BattleController battleController;
	
	/// <summary>
	/// Reference to character's collider.
	/// </summary>
	private Collider2D collider;
	
	/// <summary>
	/// Number of rounds after which critically wounded character dies from wounds.
	/// </summary>
	private int roundsToDeath = -1;
	
	/// <summary>
	/// Flag used to signal whether hover with this character's info is active.
	/// </summary>
	private bool hover = false;
	public delegate void OnHoverDelegate(Character c);
	
	/// <summary>
	/// Event which is emitted when mouse enters character collider.
	/// </summary>
	public event OnHoverDelegate onHover;
	
	/// <summary>
	/// Event which is emitted when mouse leaves character collider.
	/// </summary>
	public event OnHoverDelegate onExit;

	/// <summary>
	/// Sets default values for some attributes and obtains character collider. Also instantiates prefab skills to
	/// prevent overwriting prefab values during playing. If character is player (availableSkills.Count > 0), only
	/// <c>BasicAttack</c> from <c>skills</c> is instantiated, because it is not chosen.
	/// </summary>
	void Awake() {
		Health = maxHealth;
		collider = GetComponent<Collider2D>();
		battleController = GameMaster.instance.gameObject.GetComponent<BattleController>();

		// Instantiate all skills because this is enemy character
		if (availableSkills.Count == 0) {
			for (int i = 0; i < skills.Count; ++i)
				skills[i] = Instantiate(skills[i], transform);
		}
		else {
			// Instantiate only basic attack because it is not added as other skills
			int i = skills.FindIndex(x => x is BasicAttack);
			skills[i] = Instantiate(skills[i], transform);
		}
	}

	/// <summary>
	/// Update is used to check whether mouse entered/left character collider and therefore whether to emit onHover or
	/// onExit events, which turn on/off hover with character information.
	/// </summary>
     void Update() {
	     if (Camera.main != null) {
		     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		     RaycastHit2D rayHit = Physics2D.GetRayIntersection(ray);

		     if (rayHit != null && rayHit.collider == collider) {
			     if (Input.GetMouseButtonDown(0)) {
				     OnMouseDownRayCast();
			     }
			     else if (!hover) {
				     hover = true;
				     OnHover();
			     }
		     }
		     else if (rayHit != null && rayHit.collider != collider && hover) {
			     hover = false;
			     OnExit();
		     }
	     }
     }

	/// <summary>
	/// Adds new effect to list of active effects.
	/// </summary>
	/// <param name="effect">New effect which will affect the character.</param>
	public void AddEffect(Effect effect) {
		activeEffects.Add(effect);
	}

	/// <summary>
	/// Returns list of active effects.
	/// </summary>
	/// <returns>List of active effects.</returns>
	public List<Effect> GetEffects() {
		return activeEffects;
	}

	/// <summary>
	/// Applies all effects at end of round. If duration of effect is 0, it is deactivated, removed from list and its
	/// game object is destroyed.
	/// </summary>
	public void ApplyEffects() {
		int i = 0;
		while (i < activeEffects.Count) {
			if (activeEffects[i].duration == 0) {
				Effect effect = activeEffects[i];
				effect.Deactivate(this);
				activeEffects.RemoveAt(i);
				Destroy(effect.gameObject);
				continue;
			}
			activeEffects[i].AtRoundEnd(this);
			i++;
		}
	}

	/// <summary>
	/// Applies damage to character. Damage can be reduced/amplified by defence. If character health after damage
	/// becomes 0, character becomes critically wounded if it is not already, else it dies. Enemy characters die
	/// instantly. Also performs action on each active effect, which might result in deactivation of some effects, such
	/// as Weaken.
	/// </summary>
	/// <param name="damage">Damage dealt by skill. Can be increased or decreased based on character defence.</param>
	/// <returns>Final damage, which character received after reduction by defence.</returns>
	public int DecreaseHealth(int damage) {
		int finalDamage = (int)Math.Round((1 - Defence) * damage);
		finalDamage = finalDamage > Health ? Health : finalDamage;
		Health -= finalDamage;
		if (Health <= 0) {
			if (!alreadyRevived && playable && !isCriticallyWounded && Team.Characters.Count > 1) {
				isCriticallyWounded = true;
				roundsToDeath = 2;
				Health = 0;
				battleController.Log = $"{name} is critically wounded and will die in {roundsToDeath} rounds.";
				Team.RemoveUnplayedCharacter(this);
				Team.AddPlayedCharacter(this);
			}
			else {
				battleController.Log = $"{name} died.";
				Team.RemoveCharacterFromTeam(this);
				Destroy(gameObject);
			}
		}

		foreach (Effect effect in activeEffects)
			effect.AfterDamage(this, finalDamage);

		return finalDamage;
	}

	/// <summary>
	/// Increases health of the character up to maxHealth.
	/// </summary>
	/// <param name="health">Amount by which health will be increased.</param>
	public void IncreaseHealth(int health) {
		Health += health;
		Health = Health > maxHealth ? maxHealth : Health;
	}

	/// <summary>
	/// Adds new skill to skills list. Used when creating new team.
	/// </summary>
	/// <param name="skill">Skill to be added to skills list.</param>
	/// <exception cref="ArgumentException">If skill is not one of character's available skills, exception is thrown.
	/// </exception>
	public void AddSkill(Skill skill) {
		if (!availableSkills.Contains(skill))
			throw new System.ArgumentException(
				$"Trying to add {skill.name}, which is not available for current character."
			);

		skills.Add(Instantiate(skill, transform));
		--SkillPoints;
	}

	/// <summary>
	/// Removes skill from skills list. Used when creating new team.
	/// </summary>
	/// <param name="skill">Skill to be removed from skills list.</param>
	public void RemoveSkill(Skill skill) {
		int i = skills.FindIndex(x => x.skillName == skill.skillName);
		Destroy(skills[i].gameObject);
		skills.RemoveAt(i);
		++SkillPoints;
	}

	/// <summary>
	/// Initializes character for battle.
	/// </summary>
	public void InitializeForBattle() {
		InBattle = true;
		Defence = 0;
	}

	/// <summary>
	/// Resets character attributes after battle.
	/// </summary>
	public void AfterBattle() {
		InBattle = false;
		Health = maxHealth;
		isCriticallyWounded = false;
		alreadyRevived = false;
		roundsToDeath = -1;
		foreach (Skill skill in skills) {
			skill.cooldown = 0;
		}
		activeEffects = new List<Effect>();
		Defence = 0;
	}

	/// <summary>
	/// Method which is called when character is clicked. If in battle, character may be revived if critically wounded,
	/// chosen as attacking character if it was not played already or it can be chosen as target of a skill if skill was
	/// selected and character is one of valid targets.
	/// </summary>
	private void OnMouseDownRayCast() {
		if (InBattle == false)
			return;

		Team currentTeam = battleController.GetCurrentTeam();

		if (isCriticallyWounded && currentTeam.Characters.Contains(this) && currentTeam.Characters.Count > 1) {
			ReviveCharacter();
			battleController.ChosenCharacter = this;
			return;
		}

		if (battleController.ChosenCharacter == null && currentTeam.Characters.Contains(this)) {
			if (currentTeam.UnplayedCharacters.Contains(this))
				battleController.ChosenCharacter = this;
			else
				battleController.Log = $"Character {characterName} was already used.";
		}
		else if (battleController.ChosenSkill != null && battleController.ValidTargets.Contains(this) &&
		         !battleController.ChosenTargets.Contains(this))
			battleController.ChosenTargets.Add(this);
	}

	/// <summary>
	/// Revives critically wounded character with half of its max health.
	/// </summary>
	private void ReviveCharacter() {
		isCriticallyWounded = false;
		alreadyRevived = true;
		battleController.CharacterRevived = true;
		Health = maxHealth / 2;
		battleController.Log = $"{name} was revived.";
	}

	/// <summary>
	/// Controls whether character will die from wounds at the end of round when critically wounded for too long.
	/// </summary>
	public void DecreaseRoundsToDeath() {
		if (isCriticallyWounded) {
			if (roundsToDeath == 0) {
				Team.RemoveCharacterFromTeam(this);
				Destroy(gameObject);
				battleController.Log = $"{name} died from wounds.";
				return;
			}

			battleController.Log = $"{name} is critically wounded and will die in {roundsToDeath} rounds.";
			roundsToDeath--;
		}
	}

	/// <summary>
	/// AI method used when enemy chooses which skill to use.
	/// </summary>
	public void SelectSkill() {
		Random rng = new Random();
		List<Skill> skillsNotOnCooldown = skills.FindAll(s => s.cooldown == 0);
		int idx = rng.Next(skillsNotOnCooldown.Count);
		battleController.ChosenSkill = skillsNotOnCooldown[idx];
	}

	/// <summary>
	/// Increases number of skill points character has and therefore skills. Is called when player reaches end of
	/// environment.
	/// </summary>
	public void AddSkillPoint() {
		++SkillPoints;
	}

	/// <summary>
	/// Increases level of character.
	/// </summary>
	/// <param name="healthIncPerLevel">Value by which max health will increase.</param>
	/// <param name="strengthIncPerLevel">Value by which base strength will increase.</param>
	/// <param name="nLevels">Value representing how much levels were gained. AI can have values larger than 1.</param>
	public void LevelUp(float healthIncPerLevel, float strengthIncPerLevel, int nLevels = 1) {
		maxHealth += (int) Math.Round(maxHealth * healthIncPerLevel * nLevels);
		Health = maxHealth;
		baseStrength += (int) Math.Round(baseStrength * strengthIncPerLevel * nLevels);
	}

	/// <summary>
	/// Decreases cooldown of each skill at end of round.
	/// </summary>
	public void DecreaseCooldowns() {
		foreach (Skill skill in skills) {
			skill.DecreaseCooldown();
		}
	}

	/// <summary>
	/// Emits onHover event after mouse enters character collider.
	/// </summary>
	private void OnHover() {
		onHover?.Invoke(this);
	}

	/// <summary>
	/// Emits onExit event after mouse leaves character collider.
	/// </summary>
	private void OnExit() {
		onExit?.Invoke(this);
	}

	/// <summary>
	/// Scales AI character attributes and skills to player level and area. This prevents game to become too easy.
	/// </summary>
	/// <param name="level">Value representing player level.</param>
	/// <param name="area">Value representing how many areas player has passed.</param>
	public void ScaleAICharacterToPlayerLevelAndArea(int level, int area) {
		LevelUp(healthIncPerLevelAI, strengthIncPerLevelAI, --level);
		foreach (Skill skill in skills)
			skill.Improve(--area);
	}

	/// <summary>
	/// Activates all effects before character applies some skill to activate buffs.
	/// </summary>
	/// <param name="s">Skill which is used.</param>
	/// <returns>List of character's active effects.</returns>
	public List<Effect> ProcessBuffs(Skill s) {
		foreach (Effect effect in activeEffects)
			effect.Activate(this, s);

		return activeEffects;
	}

	/// <summary>
	/// Overrides default ToString to return only character's name.
	/// </summary>
	/// <returns>Character's name.</returns>
	public override string ToString() {
		return characterName;
	}
}
