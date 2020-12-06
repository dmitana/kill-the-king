using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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

	private int level = 1;
	private int exp = 0;

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
        foreach (Character character in Characters) {
            character.gameObject.GetComponent<Transform>().SetParent(transform);
            character.gameObject.GetComponent<FixedJoint2D>().connectedBody = gameObject.GetComponent<Rigidbody2D>();
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
        Debug.Log("Unplayed characters");
        foreach (Character c in UnplayedCharacters) {
            Debug.Log(c.characterName);
        }
    }

	public void IncreaseEnvironment() {
		++CurrentEnvironment;
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

	public void InitilizeForBattle(BattleController controller) {
		BattleController = controller;
        foreach (Character c in Characters) {
            c.InitializeForBattle(controller);
        }
	}

	public void AddExp(int exp) {
		this.exp += exp;
		if (this.exp < expPerLevel)
			return;

		this.exp -= expPerLevel;
		level += 1;
		foreach (Character character in Characters) {
			character.maxHealth += (int) Math.Round(character.maxHealth * healthIncPerLevel);
			character.baseStrength += (int) Math.Round(character.baseStrength * strengthIncPerLevel);
		}


	}
}
