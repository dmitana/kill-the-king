using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {
	public static Team playerTeamInstance;
    public Camera teamCamera;

    private List<Character> characters = new List<Character>();
    private List<Character> playedCharacters = new List<Character>();
    private List<Character> unplayedCharacters = new List<Character>();

    void Awake() {
		if (playerTeamInstance == null && gameObject.tag == "PlayerTeam") {
			playerTeamInstance = this;
			DontDestroyOnLoad(playerTeamInstance);
		}
		else if (playerTeamInstance != null && gameObject.tag == "PlayerTeam")
			Destroy(gameObject);
    }

    public void AddCharacterToTeam(Character c) {
        characters.Add(c);
    }

    public void RemoveCharacterFromTeam(Character c) {
        characters.Remove(c);
    }

    public void SetCharactersParent() {
        foreach (Character character in characters)
            character.gameObject.GetComponent<Transform>().SetParent(transform);
    }

    public void SetActiveCharacters(Boolean value) {
        foreach (Character c in characters)
            c.gameObject.SetActive(value);
    }

    public void SetActiveCamera(Boolean value) {
        teamCamera.gameObject.SetActive(value);
    }

    public List<Character> GetCharacters() {
        return playedCharacters;
    }

    public List<Character> GetPlayedCharacters() {
        return playedCharacters;
    }

    public void AddPlayedCharacter(Character c) {
        playedCharacters.Add(c);
    }

    public void RemovePlayedCharacter(Character c) {
        playedCharacters.Remove(c);
    }

    public void AddUnplayedCharacter(Character c) {
        unplayedCharacters.Add(c);
    }

    public void RemoveUnplayedCharacter(Character c) {
        unplayedCharacters.Remove(c);
    }

    public List<Character> GetUnplayedCharacters() {
        return unplayedCharacters;
    }

    public void HighlightUnplayed() {
        Debug.Log("Unplayed characters");
        foreach (Character c in unplayedCharacters) {
            Debug.Log(c.characterName);
        }
    }
}
