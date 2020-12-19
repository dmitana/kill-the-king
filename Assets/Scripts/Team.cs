using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;

public class Team : MonoBehaviour {
	public static Team playerTeamInstance;

    public Camera teamCamera;
    public bool isAI = false;
	[Space]
	public int expPerLevel;
	public float healthIncPerLevel;
	public float strengthIncPerLevel;

    public BattleController BattleController { get; set; }
    public List<Character> Characters { get; private set; }
    public List<Character> PlayedCharacters { get; private set; }
    public List<Character> UnplayedCharacters { get; private set; }
	public int CurrentEnvironment { get; private set; } = 0;
	public int CurrentArea { get; private set; } = 0;
	public List<EnvironmentPath> Paths { get; private set; } = new List<EnvironmentPath>();
	public int Level { get; private set; } = 1;
	public int Exp { get; private set; } = 0;

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

    public void RemoveCharacterFromTeam(Character c) {
        Characters.Remove(c);
		c.Team = null;

        if (UnplayedCharacters.Contains(c))
            UnplayedCharacters.Remove(c);
        if (PlayedCharacters.Contains(c))
            PlayedCharacters.Remove(c);
    }

	public void ClearCharacters() {
		Characters.Clear();
	}

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

    public void RemovePlayedCharacter(Character c) {
        PlayedCharacters.Remove(c);
    }

    public void AddUnplayedCharacter(Character c) {
        UnplayedCharacters.Add(c);
    }

    public void RemoveUnplayedCharacter(Character c) {
        UnplayedCharacters.Remove(c);
    }

    public void HighlightUnplayed() {
        BattleController.Log = $"Unplayed characters: {string.Join(", ", UnplayedCharacters)}";
    }

	public void IncreaseEnvironment(EnvironmentPath path) {
		++CurrentEnvironment;
		Paths.Add(path);
		IncreaseArea();
	}

	public void IncreaseArea() {
		++CurrentArea;
	}

    public void SelectCharacter() {
        int idx = rng.Next(UnplayedCharacters.Count);
        BattleController.ChosenCharacter = UnplayedCharacters[idx];
    }

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

	public void InitilizeForBattle(BattleController controller, Vector3? position = null) {
		BattleController = controller;
		if (movement != null)
			movement.enabled = false;
		positionBeforeBattle = transform.position;
		if (position.HasValue) {
			transform.position = position.Value;
		}

        foreach (Character c in Characters) {
            c.InitializeForBattle(controller);
        }
	}

	public void ResetTeam(bool resetCooldowns=false) {
		PlayedCharacters = new List<Character>();
		UnplayedCharacters = new List<Character>();
		foreach (Character c in Characters) {
			if (c.IsStunned) {
				AddPlayedCharacter(c);
				c.IsStunned = false;
			}
			else
				AddUnplayedCharacter(c);
			if (resetCooldowns)
				c.DecreaseCooldowns();
			c.DecreaseRoundsToDeath();
		}
	}

	public void AfterBattle() {
		BattleController = null;
		if (movement != null)
			movement.enabled = true;
		transform.position = positionBeforeBattle;

        foreach (Character c in Characters) {
            c.AfterBattle();
        }
	}

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

	public void ApplyEffects() {
		for (int i = Characters.Count - 1; i >= 0; i--) {
			Characters[i].ApplyEffects();
		}
	}

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
