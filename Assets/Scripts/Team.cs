using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {
	private List<Character> characters = new List<Character>();
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
}
