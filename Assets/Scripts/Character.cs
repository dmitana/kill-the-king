using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour {
	public string characterName;
	public Sprite characterImg;
	public int maxHealth;
	public int baseStrength;
	public double hitChance = 1.0;
	public List<Skill> availableSkills = new List<Skill>();
	public List<Skill> skills = new List<Skill>();
	public float healthIncPerLevelAI;
	public float strengthIncPerLevelAI;

	public int SkillPoints { get; private set; } = 1;
	public Team Team { get; set; }
	public int Health { get; set; }
	public bool InBattle { get; private set; } = false;
	public double Defence { get; set; }
	public bool playable;
	public bool isCriticallyWounded;
	public bool alreadyRevived;
	public bool IsStunned { get; set; } = false;
	public bool IsInvisible { get; set; } = false;

	private List<Effect> activeEffects = new List<Effect>();
	private BattleController battleController;
	private Collider2D collider;
	private int roundsToDeath;

	public delegate void OnClickDelegate(Character c);
	public event OnClickDelegate onHover;
	public event OnClickDelegate onExit;

	private bool hover;

	void Awake() {
		Health = maxHealth;
		isCriticallyWounded = false;
		alreadyRevived = false;
		hover = false;
		roundsToDeath = -1;

		collider = GetComponent<Collider2D>();

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

	public void AddEffect(Effect effect) {
		activeEffects.Add(effect);
	}

	public List<Effect> GetEffects() {
		return activeEffects;
	}

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

	public void IncreaseHealth(int health) {
		Health += health;
		Health = Health > maxHealth ? maxHealth : Health;
	}

	public void AddSkill(Skill skill) {
		if (!availableSkills.Contains(skill))
			throw new System.ArgumentException(
				$"Trying to add {skill.name}, which is not available for current character."
			);

		skills.Add(Instantiate(skill, transform));
		--SkillPoints;
	}

	public void RemoveSkill(Skill skill) {
		int i = skills.FindIndex(x => x.skillName == skill.skillName);
		Destroy(skills[i].gameObject);
		skills.RemoveAt(i);
		++SkillPoints;
	}

	public void InitializeForBattle(BattleController controller) {
		battleController = controller;
		InBattle = true;
		Defence = 0;
	}

	public void AfterBattle() {
		battleController = null;
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

	private void ReviveCharacter() {
		isCriticallyWounded = false;
		alreadyRevived = true;
		battleController.CharacterRevived = true;
		Health = maxHealth / 2;
		battleController.Log = $"{name} was revived.";
	}

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

	public void SelectSkill() {
		Random rng = new Random();
		List<Skill> skillsNotOnCooldown = new List<Skill>();
		foreach (Skill skill in skills) {
			if (skill.cooldown == 0)
				skillsNotOnCooldown.Add(skill);
		}
		int idx = rng.Next(skillsNotOnCooldown.Count);
		battleController.ChosenSkill = skillsNotOnCooldown[idx];
	}

	public void AddSkillPoint() {
		++SkillPoints;
	}

	public void LevelUp(float healthIncPerLevel, float strengthIncPerLevel, int nLevels = 1) {
		maxHealth += (int) Math.Round(maxHealth * healthIncPerLevel * nLevels);
		Health = maxHealth;
		baseStrength += (int) Math.Round(baseStrength * strengthIncPerLevel * nLevels);
	}

	public void DecreaseCooldowns() {
		foreach (Skill skill in skills) {
			skill.DecreaseCooldown();
		}
	}

	private void OnHover() {
		onHover?.Invoke(this);
	}

	private void OnExit() {
		onExit?.Invoke(this);
	}

	public void ScaleAICharacterToPlayerLevelAndArea(int level, int area) {
		LevelUp(healthIncPerLevelAI, strengthIncPerLevelAI, --level);
		foreach (Skill skill in skills)
			skill.Improve(--area);
	}

	public List<Effect> ProcessBuffs(Skill s) {
		List<Effect> buffsToDeactivate = new List<Effect>();
		foreach (Effect effect in activeEffects) {
			effect.Activate(this, s);
			buffsToDeactivate.Add(effect);
		}

		return buffsToDeactivate;
	}
}
