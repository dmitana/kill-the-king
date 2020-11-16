using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {
    private List<Character> characters = new List<Character>();
    private List<Character> playedCharacters = new List<Character>();
    private List<Character> unplayedCharacters = new List<Character>();
    private Camera teamCamera;

    void Awake() {
        teamCamera = gameObject.GetComponentInChildren<Camera>();
        teamCamera.gameObject.SetActive(false);
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

    public void EnableCharacters() {
        foreach (Character c in characters)
            c.gameObject.SetActive(true);
    }

    public void EnableCamera() {
        teamCamera.gameObject.SetActive(true);
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
        
    }
}