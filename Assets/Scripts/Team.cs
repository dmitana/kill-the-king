using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;

/// <summary>
/// Represents team.
/// </summary>
public class Team : MonoBehaviour {
	/// <summary>
	/// Player team singleton instance.
	/// </summary>
	public static Team playerTeamInstance;
	
    public Camera teamCamera;
	
	/// <summary>
	/// Flag representing whether team is controlled by AI.
	/// </summary>
    public bool isAI = false;
	[Space]
	
	/// <summary>
	/// Value representing how many experience is needed to gaing new level.
	/// </summary>
	public int expPerLevel = 100;
	
	/// <summary>
	/// Percentual value of how much player character health will increase when gaining new level.
	/// </summary>
	public float healthIncPerLevel;
	
	/// <summary>
	/// Percentual value of how much player character base strength will increase when gaining new level.
	/// </summary>
	public float strengthIncPerLevel;
	
    public BattleController BattleController { get; set; }
	
	/// <summary>
	/// Characters of a team.
	/// </summary>
    public List<Character> Characters { get; private set; }
	
	/// <summary>
	/// Characters that have been already played in a round.
	/// </summary>
    public List<Character> PlayedCharacters { get; private set; }
	
	/// <summary>
	/// Characters that haven't been played in a round yet.
	/// </summary>
    public List<Character> UnplayedCharacters { get; private set; }
	
	/// <summary>
	/// Number of current environment. Increments with each passed environment.
	/// </summary>
	public int CurrentEnvironment { get; private set; } = 0;
	
	/// <summary>
	/// Number of current area in current environment. Increments with each passed area and resets in a new environment.
	/// </summary>
	public int CurrentArea { get; private set; } = 0;
	
	/// <summary>
	/// List of paths player chose. It is used in map.
	/// </summary>
	public List<EnvironmentPath> Paths { get; private set; } = new List<EnvironmentPath>();
	
	/// <summary>
	/// Current player level.
	/// </summary>
	public int Level { get; private set; } = 1;
	
	/// <summary>
	/// Experience points player gained on current player level.
	/// </summary>
	public int Exp { get; private set; } = 0;

	/// <summary>
	/// Position of team before entering battle. When returning from battle, it is used to place team where it was
	/// before battle.
	/// </summary>
	private Vector3 positionBeforeBattle;
	private CharacterMovement movement;

    private Random rng;
    
    void Awake() {
		if (playerTeamInstance == null && gameObject.tag == "PlayerTeam") {
			playerTeamInstance = this;
			DontDestroyOnLoad(playerTeamInstance);
		}
		else if (playerTeamInstance != null && gameObject.tag == "PlayerTeam")
			Destroy(gameObject);

        Characters = new List<Character>();
        PlayedCharacters = new List<Character>();
        UnplayedCharacters = new List<Character>();
        rng = new Random();
		movement = gameObject.GetComponent<CharacterMovement>();
    }
    
    public void AddCharacterToTeam(Character c) {
        Characters.Add(c);
		c.Team = this;
    }

    /// <summary>
    /// Removes character from team.
    /// </summary>
    /// <param name="c">Character to be removed.</param>
    public void RemoveCharacterFromTeam(Character c) {
        if (Characters.Contains(c))
			Characters.Remove(c);
        if (UnplayedCharacters.Contains(c))
            UnplayedCharacters.Remove(c);
        if (PlayedCharacters.Contains(c))
            PlayedCharacters.Remove(c);
        c.Team = null;
    }
    
	public void ClearCharacters() {
		Characters.Clear();
	}

    /// <summary>
    /// Sets parent game object of each character as team game object and also moves characters to not overlap.
    /// </summary>
	public void SetCharactersParent() {
		int i = 0;
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        foreach (Character character in Characters.Reverse<Character>()) {
			character.gameObject.transform.SetParent(transform);
            character.gameObject.GetComponent<FixedJoint2D>().connectedBody = rb;

			// Move characters to not overlap
			Vector3 position = character.gameObject.transform.position;
			position[0] -= i * 2;
			character.gameObject.transform.position = position;
			++i;
        }
    }

    /// <summary>
    /// Activates/deactivates characters game objects. During team selection, characters are deactivated.
    /// </summary>
    /// <param name="value">Boolean used to activate/deactivate characters.</param>
    public void SetActiveCharacters(Boolean value) {
        foreach (Character c in Characters)
            c.gameObject.SetActive(value);
    }
    
    public void SetActiveCamera(Boolean value) {
        teamCamera.gameObject.SetActive(value);
    }
    
    public void AddPlayedCharacter(Character c) {
        PlayedCharacters.Add(c);
    }
    
    public void AddUnplayedCharacter(Character c) {
        UnplayedCharacters.Add(c);
    }
    
    public void RemoveUnplayedCharacter(Character c) {
        UnplayedCharacters.Remove(c);
    }

    /// <summary>
    /// Highlights all unplayed characters.
    /// </summary>
    public void HighlightUnplayed() {
        if (!isAI) {
	        foreach (Character c in UnplayedCharacters)
		        c.OnValid(true);
	        foreach (Character c in PlayedCharacters)
		        c.OnValid(false);
        }
    }
    
	public void IncreaseEnvironment(EnvironmentPath path) {
		++CurrentEnvironment;
		Paths.Add(path);
		IncreaseArea();
	}
    
    public void IncreaseArea() {
		++CurrentArea;
	}

	/// <summary>
	/// AI method for selecting one of unplayed characters.
	/// </summary>
    public void SelectCharacter() {
        int idx = rng.Next(UnplayedCharacters.Count);
        BattleController.ChosenCharacter = UnplayedCharacters[idx];
    }

	/// <summary>
	/// AI method for selecting targets of a skill from list of valid targets.
	/// </summary>
	/// <param name="numOfTargets">How many targets should be chosen.</param>
	/// <param name="validTargets">List of all valid characters.</param>
    public void SelectTargets(int numOfTargets, List<Character> validTargets) {
        while (numOfTargets > BattleController.ChosenTargets.Count) {
            int idx = rng.Next(validTargets.Count);
            if (!BattleController.ChosenTargets.Contains(validTargets[idx]))
                BattleController.ChosenTargets.Add(validTargets[idx]);
        }
    }
	
	public void AddSkillPointToCharacters() {
		foreach (Character character in Characters)
			character.AddSkillPoint();
	}

	/// <summary>
	/// Initializes team for battle. Movement is turned off and team is placed into correct position.
	/// </summary>
	/// <param name="position">Position to which team will be placed.</param>
	public void InitilizeForBattle(Vector3? position = null) {
		BattleController = GameMaster.instance.gameObject.GetComponent<BattleController>();
		if (movement != null)
			movement.enabled = false;
		positionBeforeBattle = transform.position;
		if (position.HasValue) {
			transform.position = position.Value;
		}

        foreach (Character c in Characters) {
            c.InitializeForBattle();
        }
	}

	/// <summary>
	/// Resets team. All characters become unplayed except stunned characters, which become played. Also cooldowns and
	/// rounds to death are decreased.
	/// </summary>
	/// <param name="resetCooldowns">Cooldowns are reset only at round end.</param>
	public void ResetTeam(bool resetCooldowns=false) {
		PlayedCharacters = new List<Character>();
		UnplayedCharacters = new List<Character>();
		for (int i = Characters.Count - 1; i >= 0; i--) {
			if (Characters[i].IsStunned || Characters[i].isCriticallyWounded) {
				AddPlayedCharacter(Characters[i]);
				Characters[i].IsStunned = false;
			}
			else
				AddUnplayedCharacter(Characters[i]);
			if (resetCooldowns)
				Characters[i].DecreaseCooldowns();
			Characters[i].DecreaseRoundsToDeath();
		}
	}

	/// <summary>
	/// Restores movement and position of team before battle. Also restores characters attributes.
	/// </summary>
	public void AfterBattle() {
		BattleController = null;
		if (movement != null)
			movement.enabled = true;
		transform.position = positionBeforeBattle;

        foreach (Character c in Characters) {
            c.AfterBattle();
        }
	}

	/// <summary>
	/// Adds experience to team after battle or successful event. Also increases player level if enough experience was
	/// obtained.
	/// </summary>
	/// <param name="exp">Gained experience.</param>
	public void AddExp(int exp) {
		Exp += exp;
		if (Exp < expPerLevel)
			return;

		Exp -= expPerLevel;
		Level += 1;
		foreach (Character character in Characters) {
			character.LevelUp(healthIncPerLevel, strengthIncPerLevel);
		}
	}

	/// <summary>
	/// Applies effects to all characters. Application of DamageOverTime effect can cause death of a character.
	/// </summary>
	public void ApplyEffects() {
		for (int i = Characters.Count - 1; i >= 0; i--) {
			Characters[i].ApplyEffects();
		}
	}

	/// <summary>
	/// Restart team after Game ends.
	/// </summary>
	public void Restart() {
		CurrentEnvironment = 0;
		CurrentArea = 0;
		Paths = new List<EnvironmentPath>();
		Level = 1;
		Exp = 0;
		movement.enabled = true;

		foreach (Character character in Characters)
			Destroy(character.gameObject);
        Characters = new List<Character>();

        transform.position = new Vector3(0, 0, 0);
	}
}
